using Autofac;
using DataDownloader.Test.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataDownloader.Test
{
    public abstract class ContainerBasedTestBase
    {
        public TestContainerProvider ContainerProvider { get; set; }
        public IContainer Container => ContainerProvider.Container;

        [TestInitialize]
        public virtual void TestInitialize()
        {
            ContainerProvider = new TestContainerProvider();
        }

        [TestCleanup]
        public virtual void TestCleanup()
        {
            ContainerProvider.Dispose();
        }
    }
}