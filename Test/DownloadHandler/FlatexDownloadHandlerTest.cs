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
    public class FlatexDownloadHandlerTest : ContainerBasedTestBase
    {
        public DownloadHandlerConfiguration DownloadHandlerConfiguration { get; set; }
        public FlatexDownloadHandler DownloadHandler { get; set; }
        public IBankAccountRepository AccountRepository { get; set; }
        public IPortfolioRepository PortfolioRepository { get; set; }
        public IRepository<FlatexTransactionEntity> TransactionRepository { get; set; }
        public IRepository<FlatexPositionEntity> PortfolioPositionRepository { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            base.TestInitialize();

            DownloadHandler = Container.Resolve<FlatexDownloadHandler>();
            DownloadHandlerConfiguration =
                Container.ResolveKeyed<DownloadHandlerConfiguration>(
                    Constants.UniqueContainerKeys.DownloadHandlerFlatex);
            AccountRepository = Container.Resolve<IBankAccountRepository>();
            PortfolioRepository = Container.Resolve<IPortfolioRepository>();
            TransactionRepository = Container.Resolve<IRepository<FlatexTransactionEntity>>();
            PortfolioPositionRepository = Container.Resolve<IRepository<FlatexPositionEntity>>();

            DownloadHandlerConfiguration.DownloadPath = TestConfiguration.DownloadHandler.Flatex.Path;
            DownloadHandlerConfiguration.KeePassEntryUuid = TestConfiguration.KeePass.FlatexUuid;
        }

        [TestMethod]
        public void TestFlatexInitialImport()
        {
            var account = AccountRepository.InsertOrGetWithEquality(new BankAccountEntity
            {
                BankName = Constants.DownloadHandler.BankNameFlatex,
                AccountName = Constants.DownloadHandler.AccountNameGiro,
                Iban = TestConfiguration.DownloadHandler.Flatex.GiroIban
            });
            DownloadHandler.ProcessFiles(new[]
            {
                new FileParserInput
                {
                    OwningEntity = account,
                    FileParser = Container.ResolveKeyed<IFileParser>(Constants.UniqueContainerKeys.FileParserFlatexGiro),
                    FilePath = TestConfiguration.Parser.FlatexGiroPath,
                    TargetEntity = typeof (FlatexTransactionEntity),
                    Balance = TestConfiguration.DownloadHandler.Flatex.GiroBalance,
                    BalanceSelectorFunc =
                        () =>
                            AccountRepository.GetById(account.Id).Transactions.Sum(entity => entity.Amount)
                   }
            });
            Assert.AreEqual(TestConfiguration.DownloadHandler.Flatex.GiroCount, TransactionRepository.GetAll().Count());

            var depot = PortfolioRepository.InsertOrGetWithEquality(new PortfolioEntity
            {
                PortfolioNumber = TestConfiguration.DownloadHandler.Flatex.PortfolioNumber,
                BankName = Constants.DownloadHandler.BankNameFlatex,
                AccountName = Constants.DownloadHandler.AccountNameDepot
            });
            DownloadHandler.ProcessFiles(new[]
            {
                new FileParserInput
                {
                    OwningEntity = depot,
                    FileParser = Container.ResolveKeyed<IFileParser>(Constants.UniqueContainerKeys.FileParserFlatexDepot),
                    FilePath = TestConfiguration.Parser.FlatexDepotPath,
                    TargetEntity = typeof (FlatexPositionEntity)
                   }
            });
            Assert.AreEqual(TestConfiguration.DownloadHandler.Flatex.PortfolioCount, PortfolioPositionRepository.GetAll().Count());
        }

        [TestMethod]
        public void TestFlatexExecute()
        {
            TestFlatexInitialImport();
            DownloadHandler.Execute(true);
            Assert.IsTrue(TransactionRepository.GetAll().Count() != 0);
        }
    }
}
