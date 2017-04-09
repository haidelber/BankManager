using System.Linq;
using Autofac;
using BankManager.Common;
using BankManager.Common.Model.Configuration;
using BankManager.Core.DownloadHandler.Impl;
using BankManager.Core.Model.FileParser;
using BankManager.Core.Parser;
using BankManager.Data.Entity;
using BankManager.Data.Entity.BankTransactions;
using BankManager.Data.Repository;
using BankManager.Test._Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestConfiguration = BankManager.Test._Configuration.TestConfiguration;

namespace BankManager.Test.DownloadHandler
{
    [TestClass]
    public class RaiffeisenDownloadHandlerTest : ContainerBasedTestBase
    {
        public DownloadHandlerConfiguration DownloadHandlerConfiguration { get; set; }
        public RaiffeisenDownloadHandler DownloadHandler { get; set; }
        public IRepository<RaiffeisenTransactionEntity> TransactionRepository { get; set; }
        public IBankAccountRepository BankAccountRepository { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            base.TestInitialize();

            DownloadHandler = Container.Resolve<RaiffeisenDownloadHandler>();
            DownloadHandlerConfiguration =
                Container.ResolveKeyed<DownloadHandlerConfiguration>(
                    Constants.UniqueContainerKeys.DownloadHandlerRaiffeisen);
            TransactionRepository = Container.Resolve<IRepository<RaiffeisenTransactionEntity>>();
            BankAccountRepository = Container.Resolve<IBankAccountRepository>();

            DownloadHandlerConfiguration.DownloadPath = TestConfiguration.DownloadHandler.Raiffeisen.Path;
            DownloadHandlerConfiguration.KeePassEntryUuid = TestConfiguration.KeePass.RaiffeisenUuid;
        }
        [TestMethod]
        public void TestInitialImport()
        {
            var bankAccount = BankAccountRepository.InsertOrGetWithEquality(new BankAccountEntity
            {
                Iban = TestConfiguration.DownloadHandler.Raiffeisen.GiroIban,
                BankName = Constants.DownloadHandler.BankNameRaiffeisen,
                AccountName = Constants.DownloadHandler.AccountNameGiro
            });
            DownloadHandler.ProcessFiles(new[]
            {
                new FileParserInput
                {
                    OwningEntity = bankAccount,
                    FileParser = Container.ResolveKeyed<IFileParser>(Constants.UniqueContainerKeys.FileParserRaiffeisenGiro),
                    FilePath = TestConfiguration.Parser.RaiffeisenPath,
                    TargetEntity = typeof (RaiffeisenTransactionEntity),
                    Balance = TestConfiguration.DownloadHandler.Raiffeisen.GiroBalance,
                    BalanceSelectorFunc =
                        () =>
                            BankAccountRepository.GetById(bankAccount.Id).Transactions.Sum(entity => entity.Amount)
                }
            });
            Assert.AreEqual(TestConfiguration.DownloadHandler.Raiffeisen.GiroCount, TransactionRepository.GetAll().Count());
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
