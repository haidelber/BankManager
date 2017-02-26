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
using BankDataDownloader.Data.Repository.Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace DataDownloader.Test.DownloadHandler
{
    [TestClass]
    public class Number26DownloadHandlerTest : ContainerBasedTestBase
    {
        public DownloadHandlerConfiguration DownloadHandlerConfiguration { get; set; }
        public Number26DownloadHandler DownloadHandler { get; set; }
        public IBankAccountRepository BankAccountRepository { get; set; }
        public IRepository<Number26TransactionEntity> TransactionRepository { get; set; }

        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();

            DownloadHandler = Container.Resolve<Number26DownloadHandler>();
            DownloadHandlerConfiguration =
                Container.ResolveNamed<DownloadHandlerConfiguration>(
                    Constants.UniqueContainerKeys.DownloadHandlerNumber26);
            BankAccountRepository = Container.Resolve<IBankAccountRepository>();
            TransactionRepository = Container.Resolve<IRepository<Number26TransactionEntity>>();

            DownloadHandlerConfiguration.DownloadPath = TestConstants.DownloadHandler.Number26Path;
            DownloadHandlerConfiguration.KeePassEntryUuid = TestConstants.Service.KeePass.Number26Uuid;
        }
        [TestMethod]
        public void TestInitialImport()
        {
            var bankAccount = BankAccountRepository.InsertOrGetWithEquality(new BankAccountEntity
            {
                AccountNumber = "DE10100110012624478097",
                Iban = "DE10100110012624478097",
                BankName = Constants.DownloadHandler.BankNameNumber26,
                AccountName = Constants.DownloadHandler.AccountNameGiro
            });
            DownloadHandler.ProcessFiles(new[]
            {
                new FileParserInput
                {
                    OwningEntity = bankAccount,
                    FileParser = Container.ResolveNamed<IFileParser>(Constants.UniqueContainerKeys.FileParserNumber26),
                    FilePath = TestConstants.Parser.CsvParser.Number26Path,
                    TargetEntity = typeof (Number26TransactionEntity),
                    Balance = 682.12M,
                    BalanceSelectorFunc =
                        () =>
                            BankAccountRepository.GetById(bankAccount.Id).Transactions.Sum(entity => entity.Amount)
                   }
            });
            AreEqual(115, TransactionRepository.GetAll().Count());
        }

        [TestMethod]
        public void TestExecute()
        {
            TestInitialImport();
            DownloadHandler.Execute(true);
            IsTrue(TransactionRepository.GetAll().Count() != 0);
        }
    }
}
