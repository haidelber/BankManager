using BankDataDownloader.Common;
using BankDataDownloader.Common.Model.Configuration;
using BankDataDownloader.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BankDataDownloader.Test.Data
{
    public abstract class DataTestBase : ContainerBasedTestBase
    {
        public DatabaseConfiguration DatabaseConfiguration { get; set; }
        public DataContext DataContext { get; set; }

        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();
            DatabaseConfiguration = new DatabaseConfiguration
            {
                DatabasePath = TestConstants.Data.DatabasePath
            };
            DataContext = new DataContext(DatabaseConfiguration, ContainerProvider.Container);
        }

        [TestCleanup]
        public override void TestCleanup()
        {
            DataContext.Dispose();
            base.TestCleanup();
        }
    }
}