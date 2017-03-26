using System.Linq;
using Autofac;
using BankDataDownloader.Common;
using BankDataDownloader.Common.Model.Configuration;
using BankDataDownloader.Core.DownloadHandler.Impl;
using BankDataDownloader.Core.Model;
using BankDataDownloader.Core.Model.FileParser;
using BankDataDownloader.Core.Parser;
using BankDataDownloader.Data.Entity;
using BankDataDownloader.Data.Entity.BankTransactions;
using BankDataDownloader.Data.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BankDataDownloader.Test.DownloadHandler
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

            DownloadHandlerConfiguration.DownloadPath = TestConstants.DownloadHandler.DkbPath;
            DownloadHandlerConfiguration.KeePassEntryUuid = TestConstants.Service.KeePass.DkbUuid;
        }
        [TestMethod]
        public void TestInitialImport()
        {
            var creditCard = CreditCardAccountRepository.InsertOrGetWithEquality(new CreditCardAccountEntity
            {
                AccountNumber = "49984108",
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
                    FilePath = TestConstants.Parser.CsvParser.DkbCreditPath,
                    TargetEntity = typeof (DkbCreditTransactionEntity),
                    Balance =  944.4M,
                    BalanceSelectorFunc =
                        () =>
                            CreditCardAccountRepository.GetById(creditCard.Id).Transactions.Sum(entity => entity.Amount)
                   }
            });
            Assert.AreEqual(330, CreditTransactionRepository.GetAll().Count());

            var bankAccount = BankAccountRepository.InsertOrGetWithEquality(new BankAccountEntity
            {
                AccountNumber = "DE08120300001018630648",
                Iban = "DE08120300001018630648",
                BankName = Constants.DownloadHandler.BankNameDkb,
                AccountName = Constants.DownloadHandler.AccountNameGiro
            });
            DownloadHandler.ProcessFiles(new[]
            {
                new FileParserInput
                {
                    OwningEntity = bankAccount,
                    FileParser = Container.ResolveKeyed<IFileParser>(Constants.UniqueContainerKeys.FileParserDkbGiro),
                    FilePath = TestConstants.Parser.CsvParser.DkbGiroPath,
                    TargetEntity = typeof (DkbTransactionEntity),
                    Balance = 0.01M,
                    BalanceSelectorFunc =
                        () =>
                            BankAccountRepository.GetById(bankAccount.Id).Transactions.Sum(entity => entity.Amount)
                   }
            });
            Assert.AreEqual(48, TransactionRepository.GetAll().Count());
        }

        [TestMethod]
        public void TestExecute()
        {
            TestInitialImport();
            DownloadHandler.Execute(true);
            Assert.IsTrue(TransactionRepository.GetAll().Count() != 0);
            Assert.IsTrue(CreditTransactionRepository.GetAll().Count() != 0);
        }
    }
}
