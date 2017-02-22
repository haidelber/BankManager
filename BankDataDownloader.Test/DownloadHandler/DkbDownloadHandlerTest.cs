using System.Linq;
using Autofac;
using BankDataDownloader.Common;
using BankDataDownloader.Common.Model.Configuration;
using BankDataDownloader.Core.DownloadHandler.Impl;
using BankDataDownloader.Core.Model;
using BankDataDownloader.Core.Parser;
using BankDataDownloader.Data.Entity;
using BankDataDownloader.Data.Entity.BankTransactions;
using BankDataDownloader.Data.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace DataDownloader.Test.DownloadHandler
{
    [TestClass]
    public class DkbDownloadHandlerTest : ContainerBasedTestBase
    {
        public DownloadHandlerConfiguration DownloadHandlerConfiguration { get; set; }
        public DkbDownloadHandler DownloadHandler { get; set; }
        public IBankAccountRepository BankAccountRepository { get; set; }
        public ICreditCardAccountRepository CreditCardAccountRepository { get; set; }

        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();

            DownloadHandler = Container.Resolve<DkbDownloadHandler>();
            DownloadHandlerConfiguration =
                Container.ResolveNamed<DownloadHandlerConfiguration>(
                    Constants.UniqueContainerKeys.DownloadHandlerDkb);
            BankAccountRepository = Container.Resolve<IBankAccountRepository>();
            CreditCardAccountRepository = Container.Resolve<ICreditCardAccountRepository>();

            DownloadHandlerConfiguration.DownloadPath = TestConstants.DownloadHandler.DkbPath;
            DownloadHandlerConfiguration.KeePassEntryUuid = TestConstants.Service.KeePass.DkbUuid;
        }
        [TestMethod]
        public void TestInitialImport()
        {
            var creditCard = CreditCardAccountRepository.InsertOrGetWithEquality(new CreditCardAccountEntity
            {
                AccountNumber = "AT033477700008127839",
                CreditCardNumber = null, 
                BankName = Constants.DownloadHandler.BankNameDkb,
                AccountName = Constants.DownloadHandler.AccountNameVisa
            });
            DownloadHandler.ProcessFiles(new[]
            {
                new FileParserInput
                {
                    OwningEntity = creditCard,
                    FileParser = Container.ResolveNamed<IFileParser>(Constants.UniqueContainerKeys.FileParserDkbCredit),
                    FilePath = TestConstants.Parser.CsvParser.DkbCreditPath,
                    TargetEntity = typeof (DkbCreditTransactionEntity),
                   }
            });
            //TODO AreNotEqual(0, TransactionRepository.GetAll().Count());
        }

        [TestMethod]
        public void TestExecute()
        {
            TestInitialImport();
            DownloadHandler.Execute(true);
            //TODO IsTrue(TransactionRepository.GetAll().Count() != 0);
        }
    }
}
