using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BankDataDownloader.Test.Data
{
    [TestClass]
    public class BasicDataTest : DataTestBase
    {
        [TestMethod]
        public void TestAvailabilityOfSqLite()
        {
            var trans = DataContext.BankTransactions.ToList();
        }
    }
}
