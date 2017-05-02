using BankManager.Common.Model.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BankManager.Test.Data
{
    public abstract class DataTestBase : ContainerBasedTestBase
    {
        public DatabaseConfiguration DatabaseConfiguration { get; set; }

        public virtual void TestInitialize()
        {
            base.TestInitialize();
        }

        public override void TestCleanup()
        {
            DataContext.Dispose();
            base.TestCleanup();
        }
    }
}