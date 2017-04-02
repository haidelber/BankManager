using Autofac;
using BankManager.Common.Model.Configuration;
using BankManager.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BankManager.Test.Data
{
    public abstract class DataTestBase : ContainerBasedTestBase
    {
        public DatabaseConfiguration DatabaseConfiguration { get; set; }

        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();
        }

        [TestCleanup]
        public override void TestCleanup()
        {
            DataContext.Dispose();
            base.TestCleanup();
        }
    }
}