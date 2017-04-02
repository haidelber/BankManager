﻿using System.Linq;
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
    public class FlatexDownloadHandlerTest : ContainerBasedTestBase
    {
        public DownloadHandlerConfiguration DownloadHandlerConfiguration { get; set; }
        public FlatexDownloadHandler DownloadHandler { get; set; }
        public IBankAccountRepository AccountRepository { get; set; }
        public IPortfolioRepository PortfolioRepository { get; set; }
        public IRepository<FlatexTransactionEntity> TransactionRepository { get; set; }
        public IRepository<FlatexPositionEntity> PortfolioPositionRepository { get; set; }

        [TestInitialize]
        public override void TestInitialize()
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

            DownloadHandlerConfiguration.DownloadPath = TestConstants.DownloadHandler.FlatexPath;
            DownloadHandlerConfiguration.KeePassEntryUuid = TestConstants.Service.KeePass.FlatexUuid;
        }

        [TestMethod]
        public void TestInitialImport()
        {
            var account = AccountRepository.InsertOrGetWithEquality(new BankAccountEntity
            {
                BankName = Constants.DownloadHandler.BankNameFlatex,
                AccountName = Constants.DownloadHandler.AccountNameGiro,
                AccountNumber = "31009812558",
                Iban = "AT861948031009812558"
            });
            DownloadHandler.ProcessFiles(new[]
            {
                new FileParserInput
                {
                    OwningEntity = account,
                    FileParser = Container.ResolveKeyed<IFileParser>(Constants.UniqueContainerKeys.FileParserFlatexGiro),
                    FilePath = TestConstants.Parser.CsvParser.FlatexGiroPath,
                    TargetEntity = typeof (FlatexTransactionEntity),
                    Balance = 1721.65M,
                    BalanceSelectorFunc =
                        () =>
                            AccountRepository.GetById(account.Id).Transactions.Sum(entity => entity.Amount)
                   }
            });
            Assert.AreEqual(5, TransactionRepository.GetAll().Count());

            var depot = PortfolioRepository.InsertOrGetWithEquality(new PortfolioEntity
            {
                PortfolioNumber = "31009812566",
                BankName = Constants.DownloadHandler.BankNameFlatex,
                AccountName = Constants.DownloadHandler.AccountNameDepot
            });
            DownloadHandler.ProcessFiles(new[]
            {
                new FileParserInput
                {
                    OwningEntity = depot,
                    FileParser = Container.ResolveKeyed<IFileParser>(Constants.UniqueContainerKeys.FileParserFlatexDepot),
                    FilePath = TestConstants.Parser.CsvParser.FlatexDepotPath,
                    TargetEntity = typeof (FlatexPositionEntity)
                   }
            });
            Assert.AreEqual(5, PortfolioPositionRepository.GetAll().Count());
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
