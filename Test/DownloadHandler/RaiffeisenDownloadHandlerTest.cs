using System.Linq;
using Autofac;
using BankDataDownloader.Common;
using BankDataDownloader.Common.Model.Configuration;
using BankDataDownloader.Core.DownloadHandler.Impl;
using BankDataDownloader.Core.Model;
using BankDataDownloader.Core.Model.FileParser;
using BankDataDownloader.Core.Parser;
using BankDataDownloader.Data.Entity;
using BankDataDownloader.Data.Entity.BankTransactions;
using BankDataDownloader.Data.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BankDataDownloader.Test.DownloadHandler
{
    [TestClass]
    public class RaiffeisenDownloadHandlerTest : ContainerBasedTestBase
    {
        public DownloadHandlerConfiguration DownloadHandlerConfiguration { get; set; }
        public RaiffeisenDownloadHandler DownloadHandler { get; set; }
        public IRepository<RaiffeisenTransactionEntity> TransactionRepository { get; set; }
        public IBankAccountRepository BankAccountRepository { get; set; }

        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();

            DownloadHandler = Container.Resolve<RaiffeisenDownloadHandler>();
            DownloadHandlerConfiguration =
                Container.ResolveKeyed<DownloadHandlerConfiguration>(
                    Constants.UniqueContainerKeys.DownloadHandlerRaiffeisen);
            TransactionRepository = Container.Resolve<IRepository<RaiffeisenTransactionEntity>>();
            BankAccountRepository = Container.Resolve<IBankAccountRepository>();

            DownloadHandlerConfiguration.DownloadPath = TestConstants.DownloadHandler.RaiffeisenPath;
            DownloadHandlerConfiguration.KeePassEntryUuid = TestConstants.Service.KeePass.RaiffeisenUuid;
        }
        [TestMethod]
        public void TestInitialImport()
        {
            var bankAccount = BankAccountRepository.InsertOrGetWithEquality(new BankAccountEntity
            {
                AccountNumber = "AT033477700008127839",
                Iban = "AT033477700008127839",
                BankName = Constants.DownloadHandler.BankNameRaiffeisen,
                AccountName = Constants.DownloadHandler.AccountNameGiro
            });
            DownloadHandler.ProcessFiles(new[]
            {
                new FileParserInput
                {
                    OwningEntity = bankAccount,
                    FileParser = Container.ResolveKeyed<IFileParser>(Constants.UniqueContainerKeys.FileParserRaiffeisenGiro),
                    FilePath = TestConstants.Parser.CsvParser.RaiffeisenPath,
                    TargetEntity = typeof (RaiffeisenTransactionEntity),
                    Balance = 3599.93M,
                    BalanceSelectorFunc =
                        () =>
                            BankAccountRepository.GetById(bankAccount.Id).Transactions.Sum(entity => entity.Amount)
                }
            });
            Assert.AreEqual(1597, TransactionRepository.GetAll().Count());
        }

        [TestMethod]
        public void TestExecute()
        {
            TestInitialImport();
            DownloadHandler.Execute(true);
            Assert.IsTrue(TransactionRepository.GetAll().Count() != 0);
        }
    }
}
