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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestConfiguration = BankManager.Test._Configuration.TestConfiguration;

namespace BankManager.Test.DownloadHandler
{
    [TestClass]
    public class RciDownloadHandlerTest : ContainerBasedTestBase
    {
        public DownloadHandlerConfiguration DownloadHandlerConfiguration { get; set; }
        public RciDownloadHandler DownloadHandler { get; set; }
        public IBankAccountRepository AccountRepository { get; set; }
        public IRepository<RciTransactionEntity> TransactionRepository { get; set; }

        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();

            DownloadHandler = Container.Resolve<RciDownloadHandler>();
            DownloadHandlerConfiguration =
                Container.ResolveKeyed<DownloadHandlerConfiguration>(
                    Constants.UniqueContainerKeys.DownloadHandlerRci);
            AccountRepository = Container.Resolve<IBankAccountRepository>();
            TransactionRepository = Container.Resolve<IRepository<RciTransactionEntity>>();

            DownloadHandlerConfiguration.DownloadPath = TestConfiguration.DownloadHandler.Rci.Path;
            DownloadHandlerConfiguration.KeePassEntryUuid = TestConfiguration.KeePass.RciUuid;
        }

        [TestMethod]
        public void TestInitialImport()
        {
            var account = AccountRepository.InsertOrGetWithEquality(new BankAccountEntity
            {
                BankName = Constants.DownloadHandler.BankNameRci,
                AccountName = Constants.DownloadHandler.AccountNameSaving,
                Iban = TestConfiguration.DownloadHandler.Rci.Iban
            });
            DownloadHandler.ProcessFiles(new[]
            {
                new FileParserInput
                {
                    OwningEntity = account,
                    FileParser = Container.ResolveKeyed<IFileParser>(Constants.UniqueContainerKeys.FileParserRci),
                    FilePath = TestConfiguration.Parser.RciPath,
                    TargetEntity = typeof (RciTransactionEntity),
                    Balance = TestConfiguration.DownloadHandler.Rci.Balance,
                    BalanceSelectorFunc =
                        () =>
                            AccountRepository.GetById(account.Id).Transactions.Sum(entity => entity.Amount)
                   }
            });
            Assert.AreEqual(TestConfiguration.DownloadHandler.Rci.Count, TransactionRepository.GetAll().Count());
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
