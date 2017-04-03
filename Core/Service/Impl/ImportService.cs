using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autofac;
using BankManager.Core.Extension;
using BankManager.Core.Model.Account;
using BankManager.Core.Model.FileParser;
using BankManager.Core.Model.Import;
using BankManager.Core.Parser;
using BankManager.Data.Repository;

namespace BankManager.Core.Service.Impl
{
    public class ImportService : IImportService
    {
        public IComponentContext ComponentContext { get; }

        private Stream Source { get; set; }
        private Type TargetType { get; set; }
        private object OwningEntity { get; set; }
        private IFileParser FileParser { get; set; }
        public Func<object, object> UniqueIdGroupingFunc { get; set; }
        public List<Func<object, object>> OrderingFuncs { get; set; }

        public ImportService(IComponentContext componentContext)
        {
            ComponentContext = componentContext;
        }


        public void Import(FileParserInput input)
        {
            TargetType = input.TargetEntity;
            OwningEntity = input.OwningEntity;
            FileParser = input.FileParser;
            UniqueIdGroupingFunc = input.UniqueIdGroupingFunc;
            OrderingFuncs = input.OrderingFuncs;

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
            var toInsert = FileParser.Parse(Source).ToList();
            toInsert = Order(toInsert);
            if (UniqueIdGroupingFunc != null)
            {
                var grouping = toInsert.GroupBy(UniqueIdGroupingFunc);
                foreach (var group in grouping)
                {
                    var uniqueId = 1;
                    var subList = Order(group);
                    foreach (var bankTransactionEntity in subList)
                    {
                        var prop = bankTransactionEntity.GetType().GetProperty("UniqueId");
                        prop.SetValue(bankTransactionEntity, uniqueId++);
                    }
                }
            }
            foreach (var entity in toInsert)
            {
                accountProperty.SetValue(entity, OwningEntity);
                var persistedEntity = insertOrGetMethod.Invoke(repository, new[] { entity });
            }
            saveMethod.Invoke(repository, null);
        }

        private List<object> Order(IEnumerable<object> toOrder)
        {
            if (OrderingFuncs != null && OrderingFuncs.Count > 0)
            {
                var ordered = toOrder.OrderBy(OrderingFuncs[0]);
                for (var index = 1; index < OrderingFuncs.Count; index++)
                {
                    var orderingFunc = OrderingFuncs[index];
                    ordered = ordered.ThenBy(orderingFunc);
                }
                return ordered.ToList();
            }
            return toOrder.ToList();
        }
    }
}
