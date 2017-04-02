using System.Linq;
using BankDataDownloader.Data.Entity.BankTransactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BankDataDownloader.Test.Data
{
    [TestClass]
    public class BasicDataTest : DataTestBase
    {
        [TestMethod]
        public void TestAvailabilityOfSqLite()
        {
            var trans = DataContext.Set<RaiffeisenTransactionEntity>().ToList();
        }
    }
}
