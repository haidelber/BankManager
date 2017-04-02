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

            DownloadHandlerConfiguration.DownloadPath = TestConstants.DownloadHandler.PayPalPath;
            DownloadHandlerConfiguration.KeePassEntryUuid = TestConstants.Service.KeePass.PayPalUuid;
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
                    FilePath = TestConstants.Parser.CsvParser.PayPalPath,
                    TargetEntity = typeof (PayPalTransactionEntity),
                    Balance = 0M,
                    BalanceSelectorFunc =
                        () =>
                            AccountRepository.GetById(account.Id).Transactions.Sum(entity => entity.Amount)
                   }
            });
            Assert.AreEqual(68, TransactionRepository.GetAll().Count());
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
