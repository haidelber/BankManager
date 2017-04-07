using System;
using System.Collections.Generic;
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
    public class DkbDownloadHandlerTest : ContainerBasedTestBase
    {
        public DownloadHandlerConfiguration DownloadHandlerConfiguration { get; set; }
        public DkbDownloadHandler DownloadHandler { get; set; }
        public IBankAccountRepository BankAccountRepository { get; set; }
        public ICreditCardAccountRepository CreditCardAccountRepository { get; set; }
        public IRepository<DkbCreditTransactionEntity> CreditTransactionRepository { get; set; }
        public IRepository<DkbTransactionEntity> TransactionRepository { get; set; }

        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();

            DownloadHandler = Container.Resolve<DkbDownloadHandler>();
            DownloadHandlerConfiguration =
                Container.ResolveKeyed<DownloadHandlerConfiguration>(
                    Constants.UniqueContainerKeys.DownloadHandlerDkb);
            BankAccountRepository = Container.Resolve<IBankAccountRepository>();
            CreditCardAccountRepository = Container.Resolve<ICreditCardAccountRepository>();
            CreditTransactionRepository = Container.Resolve<IRepository<DkbCreditTransactionEntity>>();
            TransactionRepository = Container.Resolve<IRepository<DkbTransactionEntity>>();

            DownloadHandlerConfiguration.DownloadPath = TestConfiguration.DownloadHandler.Dkb.Path;
            DownloadHandlerConfiguration.KeePassEntryUuid = TestConfiguration.KeePass.DkbUuid;
        }
        [TestMethod]
        public void TestDkbInitialImport()
        {
            var creditCard = CreditCardAccountRepository.InsertOrGetWithEquality(new CreditCardEntity
            {
                AccountNumber = TestConfiguration.DownloadHandler.Dkb.CreditAccountNumber,
                CreditCardNumber = null,
                BankName = Constants.DownloadHandler.BankNameDkb,
                AccountName = Constants.DownloadHandler.AccountNameVisa
            });
            DownloadHandler.ProcessFiles(new[]
            {
                new FileParserInput
                {
                    OwningEntity = creditCard,
                    FileParser = Container.ResolveKeyed<IFileParser>(Constants.UniqueContainerKeys.FileParserDkbCredit),
                    FilePath = TestConfiguration.Parser.DkbCreditPath,
                    TargetEntity = typeof (DkbCreditTransactionEntity),
                    UniqueIdGroupingFunc = entity => ((DkbCreditTransactionEntity)entity).AvailabilityDate.Date,
                    OrderingFuncs = new List<Func<object, object>> { o => ((DkbCreditTransactionEntity)o).AvailabilityDate.Date, o => ((DkbCreditTransactionEntity)o).Text, o => ((DkbCreditTransactionEntity)o).Amount },
                    Balance =  TestConfiguration.DownloadHandler.Dkb.CreditBalance,
                    BalanceSelectorFunc =
                        () =>
                            CreditCardAccountRepository.GetById(creditCard.Id).Transactions.Sum(entity => entity.Amount)
                   }
            });
            Assert.AreEqual(TestConfiguration.DownloadHandler.Dkb.CreditCount, CreditTransactionRepository.GetAll().Count());

            var bankAccount = BankAccountRepository.InsertOrGetWithEquality(new BankAccountEntity
            {
                Iban = TestConfiguration.DownloadHandler.Dkb.GiroIban,
                BankName = Constants.DownloadHandler.BankNameDkb,
                AccountName = Constants.DownloadHandler.AccountNameGiro
            });
            DownloadHandler.ProcessFiles(new[]
            {
                new FileParserInput
                {
                    OwningEntity = bankAccount,
                    FileParser = Container.ResolveKeyed<IFileParser>(Constants.UniqueContainerKeys.FileParserDkbGiro),
                    FilePath = TestConfiguration.Parser.DkbGiroPath,
                    TargetEntity = typeof (DkbTransactionEntity),
                    UniqueIdGroupingFunc = entity => ((DkbTransactionEntity)entity).AvailabilityDate.Date,
                    OrderingFuncs = new List<Func<object, object>> { o => ((DkbTransactionEntity)o).AvailabilityDate.Date, o => ((DkbTransactionEntity)o).Text, o => ((DkbTransactionEntity)o).Amount },
                    Balance = TestConfiguration.DownloadHandler.Dkb.GiroBalance,
                    BalanceSelectorFunc =
                        () =>
                            BankAccountRepository.GetById(bankAccount.Id).Transactions.Sum(entity => entity.Amount)
                   }
            });
            Assert.AreEqual(TestConfiguration.DownloadHandler.Dkb.GiroCount, TransactionRepository.GetAll().Count());
        }

        [TestMethod]
        public void TestDkbExecute()
        {
            TestDkbInitialImport();
            DownloadHandler.Execute(true);
            Assert.IsTrue(TransactionRepository.GetAll().Count() != 0);
            Assert.IsTrue(CreditTransactionRepository.GetAll().Count() != 0);
        }
    }
}
