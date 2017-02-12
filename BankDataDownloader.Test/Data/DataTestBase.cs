using BankDataDownloader.Common.Model.Configuration;
using BankDataDownloader.Data;
using DataDownloader.Test.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataDownloader.Test.Data
{
    public abstract class DataTestBase
    {
        public DatabaseConfiguration DatabaseConfiguration { get; set; }
        public DataContext DataContext { get; set; }
        public TestContainerProvider ContainerProvider { get; set; }

        [TestInitialize]
        public virtual void TestInitialize()
        {
            DatabaseConfiguration = new DatabaseConfiguration
            {
                DatabasePath = TestConstants.Data.DatabasePath
            };
            ContainerProvider = new TestContainerProvider();
            DataContext = new DataContext(DatabaseConfiguration, ContainerProvider.Container);
        }

        [TestCleanup]
        public virtual void TestCleanup()
        {
            DataContext.Dispose();
            ContainerProvider.Dispose();
        }
    }
}