using System;
using System.Linq;
using BankDataDownloader.Data.Entity;
using BankDataDownloader.Data.Entity.BankTransactions;
using BankDataDownloader.Data.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BankDataDownloader.Test.Data
{
    [TestClass]
    public class RaiffeisenRepositoryTest : DataTestBase
    {
        public IRepository<RaiffeisenTransactionEntity> TransactionRepository { get; set; }
        public IRepository<BankAccountEntity> BankAccountRepository { get; set; }
        public BankAccountEntity BankAccountEntity { get; set; }

        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();
            TransactionRepository = new Repository<RaiffeisenTransactionEntity>(DataContext);
            BankAccountRepository = new Repository<BankAccountEntity>(DataContext);

            BankAccountEntity = new BankAccountEntity
            {
                AccountName = "Giro",
                BankName = "Raiffeisen",
                AccountNumber = "AT03000303920238080823",
                Iban = "AT03000303920238080823"
            };
            BankAccountRepository.InsertOrGetWithEquality(BankAccountEntity);
            BankAccountRepository.Save();
        }

        [TestMethod]
        public void TestDuplicates()
        {
            foreach (var raiffeisenTransactionEntities in TransactionRepository.Query()
                    .GroupBy(entity => new { entity.AvailabilityDate, entity.Amount }).Where(g => g.Count() > 1).ToList().Select(g => g.ToList()))
            {
                Console.Out.WriteLine(raiffeisenTransactionEntities);
            }
        }

        [TestMethod]
        public void TestInsert()
        {
            TransactionRepository.Insert(new RaiffeisenTransactionEntity { Amount = 10, AvailabilityDate = new DateTime(2016, 12, 31), Text = "test 1", Account = BankAccountEntity });
            Assert.AreEqual(1, TransactionRepository.QueryUnsaved().ToList().Count);
            TransactionRepository.Save();
            Assert.AreEqual(1, TransactionRepository.Query().ToList().Count);
        }
    }
}
