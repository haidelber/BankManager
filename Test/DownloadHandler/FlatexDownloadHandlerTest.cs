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

namespace BankDataDownloader.Test.DownloadHandler
{
    [TestClass]
    public class FlatexDownloadHandlerTest : ContainerBasedTestBase
    {
        public DownloadHandlerConfiguration DownloadHandlerConfiguration { get; set; }
        public FlatexDownloadHandler DownloadHandler { get; set; }
        public IBankAccountRepository AccountRepository { get; set; }
        public IPortfolioRepository PortfoliRepository { get; set; }
        

        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();

            DownloadHandler = Container.Resolve<FlatexDownloadHandler>();
            DownloadHandlerConfiguration =
                Container.ResolveKeyed<DownloadHandlerConfiguration>(
                    Constants.UniqueContainerKeys.DownloadHandlerFlatex);
            AccountRepository = Container.Resolve<IBankAccountRepository>();
            PortfoliRepository = Container.Resolve<IPortfolioRepository>();

            DownloadHandlerConfiguration.DownloadPath = TestConstants.DownloadHandler.FlatexPath;
            DownloadHandlerConfiguration.KeePassEntryUuid = TestConstants.Service.KeePass.FlatexUuid;
        }

        [TestMethod]
        [Ignore]
        public void TestInitialImport()
        {
            var account = AccountRepository.InsertOrGetWithEquality(new BankAccountEntity
            {
                BankName = Constants.DownloadHandler.BankNameRci,
                AccountName = Constants.DownloadHandler.AccountNameSaving,
                AccountNumber = "3189470019",
                Iban = "AT491942003189470019"
            });
            DownloadHandler.ProcessFiles(new[]
            {
                new FileParserInput
                {
                    OwningEntity = account,
                    FileParser = Container.ResolveKeyed<IFileParser>(Constants.UniqueContainerKeys.FileParserRci),
                    FilePath = TestConstants.Parser.CsvParser.RciPath,
                    TargetEntity = typeof (RciTransactionEntity),
                    Balance = 24731.76M,
                    BalanceSelectorFunc =
                        () =>
                            AccountRepository.GetById(account.Id).Transactions.Sum(entity => entity.Amount)
                   }
            });
            //Assert.AreEqual(12, TransactionRepository.GetAll().Count());
        }

        [TestMethod]
        public void TestExecute()
        {
            //TestInitialImport();
            DownloadHandler.Execute(true);
            //Assert.IsTrue(TransactionRepository.GetAll().Count() != 0);
        }
    }
}
