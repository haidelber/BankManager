using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataDownloader.Test.Data
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
