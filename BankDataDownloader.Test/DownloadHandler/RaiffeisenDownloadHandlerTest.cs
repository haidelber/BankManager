using System.IO;
using System.Linq;
using System.Text;
using Autofac;
using BankDataDownloader.Common;
using BankDataDownloader.Common.Model.Configuration;
using BankDataDownloader.Core.DownloadHandler;
using BankDataDownloader.Core.DownloadHandler.Impl;
using BankDataDownloader.Core.Model;
using BankDataDownloader.Core.Parser;
using BankDataDownloader.Core.ValueProvider;
using BankDataDownloader.Data.Entity;
using BankDataDownloader.Data.Entity.BankTransactions;
using BankDataDownloader.Data.Repository;
using BankDataDownloader.Data.Repository.Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace DataDownloader.Test.DownloadHandler
{
    [TestClass]
    public class RaiffeisenDownloadHandlerTest : ContainerBasedTestBase
    {
        public DownloadHandlerConfiguration DownloadHandlerConfiguration { get; set; }
        public RaiffeisenDownloadHandler RaiffeisenDownloadHandler { get; set; }
        public IRepository<RaiffeisenTransactionEntity> RaiffeisenTransactionRepository { get; set; }
        public IBankAccountRepository BankAccountRepository { get; set; }

        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();

            RaiffeisenDownloadHandler = Container.Resolve<RaiffeisenDownloadHandler>();
            DownloadHandlerConfiguration =
                Container.ResolveNamed<DownloadHandlerConfiguration>(
                    Constants.UniqueContainerKeys.DownloadHandlerRaiffeisen);
            RaiffeisenTransactionRepository = Container.Resolve<IRepository<RaiffeisenTransactionEntity>>();
            BankAccountRepository = Container.Resolve<IBankAccountRepository>();

            DownloadHandlerConfiguration.DownloadPath = TestConstants.DownloadHandler.RaiffeisenPath;
            DownloadHandlerConfiguration.KeePassEntryUuid = TestConstants.Service.KeePass.RaiffeisenUuid;
        }

        [TestMethod]
        public void Test()
        {
            var bankAccount = BankAccountRepository.InsertOrGetWithEquality(new BankAccountEntity
            {
                AccountNumber = "AT033477700008127839",
                Iban = "AT033477700008127839",
                BankName = Constants.DownloadHandler.BankNameRaiffeisen,
                AccountName = Constants.DownloadHandler.AccountNameGiro
            });
            RaiffeisenDownloadHandler.ProcessFiles(new[]
            {
                new FileParserInput
                {
                    OwningEntity = bankAccount,
                    FileParser = Container.ResolveNamed<IFileParser>(Constants.UniqueContainerKeys.FileParserRaiffeisen),
                    FilePath = TestConstants.Parser.CsvParser.RaiffeisenPath,
                    TargetEntity = typeof (RaiffeisenTransactionEntity),
                    Balance = 3599.93M,
                    CheckBalance =
                        () =>
                            BankAccountRepository.GetById(bankAccount.Id).Transactions.Sum(entity => entity.Amount) ==
                            3599.93M
                }
            });
            AreEqual(1597,RaiffeisenTransactionRepository.GetAll().Count());
            RaiffeisenDownloadHandler.Execute(true);
            IsTrue(RaiffeisenTransactionRepository.GetAll().Count() != 0);
        }
    }
}
