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

        public virtual void TestInitialize(bool useTestDatabase = false)
        {
            ConfigurationModule.ConfigurationFilePath = TestConfiguration.Configuration.Path;
            File.Delete(TestConfiguration.Configuration.Path);
            ContainerProvider = new TestContainerProvider();
            ContainerProvider.Builder.RegisterModule<DefaultServiceModule>();
            if (useTestDatabase)
            {
                ContainerProvider.Builder.RegisterModule<TestDataModule>();
            }
            else
            {
                ContainerProvider.Builder.RegisterModule<InMemoryDataModule>();
            }
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

        public virtual void TestCleanup()
        {
            ContainerProvider.Dispose();
        }
    }
}