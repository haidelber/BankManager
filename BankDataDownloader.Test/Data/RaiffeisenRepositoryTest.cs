using System;
using System.Linq;
using BankDataDownloader.Data.Entity;
using BankDataDownloader.Data.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataDownloader.Test.Data
{
    [TestClass]
    public class RaiffeisenRepositoryTest : DataTestBase
    {
        public IRepository<RaiffeisenTransactionEntity> Repository { get; set; }

        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();
            Repository = new Repository<RaiffeisenTransactionEntity>(DataContext);
        }

        [TestMethod]
        public void TestInsert()
        {
            Repository.Insert(new RaiffeisenTransactionEntity {Amount = 10,AvailabilityDate = new DateTime(2016,12,31),Text = "test 1"});
            Assert.AreEqual(1, Repository.QueryUnsaved().ToList().Count);
            Repository.Save();
            Assert.AreEqual(1, Repository.Query().ToList().Count);
        }
    }
}
