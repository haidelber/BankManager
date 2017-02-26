﻿using System.Linq;
using Autofac;
using BankDataDownloader.Common;
using BankDataDownloader.Common.Model.Configuration;
using BankDataDownloader.Core.DownloadHandler.Impl;
using BankDataDownloader.Core.Model;
using BankDataDownloader.Core.Parser;
using BankDataDownloader.Data.Entity;
using BankDataDownloader.Data.Entity.BankTransactions;
using BankDataDownloader.Data.Repository;
using BankDataDownloader.Data.Repository.Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace DataDownloader.Test.DownloadHandler
{
    [TestClass]
    public class PayPalDownloadHandlerTest : ContainerBasedTestBase
    {
        public DownloadHandlerConfiguration DownloadHandlerConfiguration { get; set; }
        public PayPalDownloadHandler DownloadHandler { get; set; }
        public IAccountRepository AccountRepository { get; set; }
        public IRepository<PayPalTransactionEntity> TransactionRepository { get; set; }

        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();

            DownloadHandler = Container.Resolve<PayPalDownloadHandler>();
            DownloadHandlerConfiguration =
                Container.ResolveNamed<DownloadHandlerConfiguration>(
                    Constants.UniqueContainerKeys.DownloadHandlerPayPal);
            AccountRepository = Container.Resolve<IAccountRepository>();
            TransactionRepository = Container.Resolve<IRepository<PayPalTransactionEntity>>();

            DownloadHandlerConfiguration.DownloadPath = TestConstants.DownloadHandler.PayPalPath;
            DownloadHandlerConfiguration.KeePassEntryUuid = TestConstants.Service.KeePass.PayPalUuid;
        }
        [TestMethod]
        public void TestInitialImport()
        {
            var account = AccountRepository.InsertOrGetWithEquality(new AccountEntity
            {
                BankName = Constants.DownloadHandler.BankNamePayPal,
                AccountName = Constants.DownloadHandler.AccountNamePaymentService
            });
            DownloadHandler.ProcessFiles(new[]
            {
                new FileParserInput
                {
                    OwningEntity = account,
                    FileParser = Container.ResolveNamed<IFileParser>(Constants.UniqueContainerKeys.FileParserPayPal),
                    FilePath = TestConstants.Parser.CsvParser.PayPalPath,
                    TargetEntity = typeof (PayPalTransactionEntity),
                    Balance = 0M,
                    BalanceSelectorFunc =
                        () =>
                            AccountRepository.GetById(account.Id).Transactions.Sum(entity => entity.Amount)
                   }
            });
            AreEqual(50, TransactionRepository.GetAll().Count());
        }

        [TestMethod]
        public void TestExecute()
        {
            //TestInitialImport();
            DownloadHandler.Execute(true);
            IsTrue(TransactionRepository.GetAll().Count() != 0);
        }
    }
}
