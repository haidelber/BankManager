using System.Linq;
using BankManager.Data.Entity.BankTransactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BankManager.Test.Data
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
