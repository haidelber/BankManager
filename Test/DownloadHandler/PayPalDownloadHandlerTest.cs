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
    public class PayPalDownloadHandlerTest : ContainerBasedTestBase
    {
        public DownloadHandlerConfiguration DownloadHandlerConfiguration { get; set; }
        public PayPalDownloadHandler DownloadHandler { get; set; }
        public IBankAccountRepository AccountRepository { get; set; }
        public IRepository<PayPalTransactionEntity> TransactionRepository { get; set; }

        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();

            DownloadHandler = Container.Resolve<PayPalDownloadHandler>();
            DownloadHandlerConfiguration =
                Container.ResolveKeyed<DownloadHandlerConfiguration>(
                    Constants.UniqueContainerKeys.DownloadHandlerPayPal);
            AccountRepository = Container.Resolve<IBankAccountRepository>();
            TransactionRepository = Container.Resolve<IRepository<PayPalTransactionEntity>>();

            DownloadHandlerConfiguration.DownloadPath = TestConfiguration.DownloadHandler.PayPal.Path;
            DownloadHandlerConfiguration.KeePassEntryUuid = TestConfiguration.KeePass.PayPalUuid;
        }
        [TestMethod]
        public void TestInitialImport()
        {
            var account = AccountRepository.InsertOrGetWithEquality(new BankAccountEntity
            {
                BankName = Constants.DownloadHandler.BankNamePayPal,
                AccountName = Constants.DownloadHandler.AccountNamePaymentService
            });
            DownloadHandler.ProcessFiles(new[]
            {
                new FileParserInput
                {
                    OwningEntity = account,
                    FileParser = Container.ResolveKeyed<IFileParser>(Constants.UniqueContainerKeys.FileParserPayPal),
                    FilePath = TestConfiguration.Parser.PayPalPath,
                    TargetEntity = typeof (PayPalTransactionEntity),
                    Balance =  TestConfiguration.DownloadHandler.PayPal.Balance,
                    BalanceSelectorFunc =
                        () =>
                            AccountRepository.GetById(account.Id).Transactions.Sum(entity => entity.Amount)
                   }
            });
            Assert.AreEqual(TestConfiguration.DownloadHandler.PayPal.Count, TransactionRepository.GetAll().Count());
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
