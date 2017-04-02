using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autofac;
using BankDataDownloader.Common.Extensions;
using BankDataDownloader.Core.Model.Account;
using BankDataDownloader.Core.Model.FileParser;
using BankDataDownloader.Core.Model.Import;
using BankDataDownloader.Core.Parser;
using BankDataDownloader.Data.Entity;
using BankDataDownloader.Data.Repository;

namespace BankDataDownloader.Core.Service.Impl
{
    public class ImportService : IImportService
    {
        public IComponentContext ComponentContext { get; }
        public IUniqueTransactionService UniqueTransactionService { get;  }

        private Stream Source { get; set; }
        private Type TargetType { get; set; }
        private object OwningEntity { get; set; }
        private IFileParser FileParser { get; set; }
        public Func<IUniqueTransactionService, IEnumerable<BankTransactionEntity>, IEnumerable<BankTransactionEntity>> AddUniqueIdFunc { get; set; }

        public ImportService(IComponentContext componentContext, IUniqueTransactionService uniqueTransactionService)
        {
            ComponentContext = componentContext;
            UniqueTransactionService = uniqueTransactionService;
        }


        public void Import(FileParserInput input)
        {
            TargetType = input.TargetEntity;
            OwningEntity = input.OwningEntity;
            FileParser = input.FileParser;
            AddUniqueIdFunc = input.AddUniqueIdFunc;

            using (Source = File.OpenRead(input.FilePath))
            {
                Import();
            }
        }

        public void Import(ImportServiceRunModel input)
        {
            TargetType = input.AccountType.GetTransactionType();
            OwningEntity = GetOwningEntity(input.AccountType, input.AccountId);
            FileParser = ComponentContext.ResolveKeyed<IFileParser>(input.FileParserConfigurationKey);

            var data = Convert.FromBase64String(input.Base64File);
            using (Source = new MemoryStream(data))
            {
                Import();
            }
        }

        private object GetOwningEntity(AccountType accountType, long accountId)
        {
            var repositoryType = typeof(IRepository<>).MakeGenericType(accountType.GetAccountType());
            var getById = repositoryType.GetMethod("GetById");
            var repository = ComponentContext.Resolve(repositoryType);
            return getById.Invoke(repository, new object[] { accountId });
        }

        private void Import()
        {
            var repositoryType = typeof(IRepository<>).MakeGenericType(TargetType);
            var insertOrGetMethod = repositoryType.GetMethod("InsertOrGetWithEquality");
            var saveMethod = repositoryType.GetMethod("Save");
            var accountProperty = TargetType.GetProperties()
                    .Single(info => info.PropertyType.IsInstanceOfType(OwningEntity));

            var repository = ComponentContext.Resolve(repositoryType);
            var toInsert =  AddUniqueIdFunc(UniqueTransactionService, (IEnumerable<BankTransactionEntity>) FileParser.Parse(Source));

            foreach (var entity in toInsert)
            {
                accountProperty.SetValue(entity, OwningEntity);
                var persistedEntity = insertOrGetMethod.Invoke(repository, new[] { entity });
            }
            saveMethod.Invoke(repository, null);
        }
    }
}
