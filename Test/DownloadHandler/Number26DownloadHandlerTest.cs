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
    public class Number26DownloadHandlerTest : ContainerBasedTestBase
    {
        public DownloadHandlerConfiguration DownloadHandlerConfiguration { get; set; }
        public Number26DownloadHandler DownloadHandler { get; set; }
        public IBankAccountRepository BankAccountRepository { get; set; }
        public ICreditCardAccountRepository CreditCardAccountRepository { get; set; }
        public IRepository<Number26TransactionEntity> TransactionRepository { get; set; }

        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();

            DownloadHandler = Container.Resolve<Number26DownloadHandler>();
            DownloadHandlerConfiguration =
                Container.ResolveKeyed<DownloadHandlerConfiguration>(
                    Constants.UniqueContainerKeys.DownloadHandlerNumber26);
            BankAccountRepository = Container.Resolve<IBankAccountRepository>();
            CreditCardAccountRepository = Container.Resolve<ICreditCardAccountRepository>();
            TransactionRepository = Container.Resolve<IRepository<Number26TransactionEntity>>();

            DownloadHandlerConfiguration.DownloadPath = TestConstants.DownloadHandler.Number26Path;
            DownloadHandlerConfiguration.KeePassEntryUuid = TestConstants.Service.KeePass.Number26Uuid;
        }
        [TestMethod]
        public void TestInitialImport()
        {
            var bankAccount = CreditCardAccountRepository.InsertOrGetWithEquality(new CreditCardEntity
            {
                AccountNumber = "DE10100110012624478097",
                BankName = Constants.DownloadHandler.BankNameNumber26,
                AccountName = Constants.DownloadHandler.AccountNameMasterCard
            });
            DownloadHandler.ProcessFiles(new[]
            {
                new FileParserInput
                {
                    OwningEntity = bankAccount,
                    FileParser = Container.ResolveKeyed<IFileParser>(Constants.UniqueContainerKeys.FileParserNumber26),
                    FilePath = TestConstants.Parser.CsvParser.Number26Path,
                    TargetEntity = typeof (Number26TransactionEntity),
                    Balance = 682.12M,
                    BalanceSelectorFunc =
                        () =>
                            CreditCardAccountRepository.GetById(bankAccount.Id).Transactions.Sum(entity => entity.Amount)
                   }
            });
            Assert.AreEqual(115, TransactionRepository.GetAll().Count());
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
