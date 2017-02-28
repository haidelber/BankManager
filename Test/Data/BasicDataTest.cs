using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BankDataDownloader.Test.Data
{
    [TestClass]
    public class BasicDataTest : DataTestBase
    {
        [TestMethod]
        public void TestAvailabilityOfSQLite()
        {
            var trans = DataContext.BankTransactions.ToList();
        }
    }
}
