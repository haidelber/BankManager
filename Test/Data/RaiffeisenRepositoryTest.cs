using System;
using System.Linq;
using Autofac;
using BankManager.Data.Entity;
using BankManager.Data.Entity.BankTransactions;
using BankManager.Data.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace BankManager.Test.Data
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
            TransactionRepository = Container.Resolve<IRepository<RaiffeisenTransactionEntity>>();
            BankAccountRepository = Container.Resolve<IRepository<BankAccountEntity>>();

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
        public void TestRaiffeisenDuplicates()
        {
            foreach (var raiffeisenTransactionEntities in TransactionRepository.Query()
                    .GroupBy(entity => new { entity.AvailabilityDate, entity.Amount }).Where(g => g.Count() > 1).ToList().Select(g => g.ToList()))
            {
                Console.Out.WriteLine(raiffeisenTransactionEntities);
            }
        }

        [TestMethod]
        public void TestRaiffeisenInsert()
        {
            TransactionRepository.Insert(new RaiffeisenTransactionEntity { Amount = 10, AvailabilityDate = new DateTime(2016, 12, 31), Text = "test 1", Account = BankAccountEntity });
            AreEqual(1, TransactionRepository.QueryUnsaved().ToList().Count);
            TransactionRepository.Save();
            AreEqual(1, TransactionRepository.Query().ToList().Count);
        }

        [TestMethod]
        public void TestRaiffeisenGetById()
        {
            TestRaiffeisenInsert();
            var acc = BankAccountRepository.GetById(BankAccountEntity.Id);
            IsNotNull(acc);
            AreEqual(acc.Transactions.Count,1);
        }
    }
}
