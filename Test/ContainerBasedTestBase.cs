using System.IO;
using Autofac;
using BankManager.Common;
using BankManager.Common.Model.Configuration;
using BankManager.Core.Configuration;
using BankManager.Core.Provider;
using BankManager.Test.Configuration;
using BankManager.Test._Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestConfiguration = BankManager.Test._Configuration.TestConfiguration;

namespace BankManager.Test
{
    public abstract class ContainerBasedTestBase
    {
        public TestContainerProvider ContainerProvider { get; set; }
        public IComponentContext Container => ContainerProvider.Container;

        public DbContext DataContext { get; set; }

        [TestInitialize]
        public virtual void TestInitialize()
        {
            ConfigurationModule.ConfigurationFilePath = TestConfiguration.Configuration.Path;
            File.Delete(TestConfiguration.Configuration.Path);
            ContainerProvider = new TestContainerProvider();
            var keePassConfiguration = Container.Resolve<KeePassConfiguration>();
            keePassConfiguration.Path = TestConfiguration.KeePass.Path;

            var keePassPasswordValueProvider = Container.Resolve<IKeePassPasswordProvider>();
            keePassPasswordValueProvider.RegisterPassword(TestConfiguration.KeePass.Password);

            var configurationService = Container.Resolve<IConfigurationProvider>();
            configurationService.ApplicationConfiguration.DatabaseConfiguration.DatabasePath =
                TestConfiguration.Database.Path;

            DataContext = Container.Resolve<DbContext>();
            DataContext.Database.Migrate();
        }

        [TestCleanup]
        public virtual void TestCleanup()
        {
            ContainerProvider.Dispose();
        }
    }
}