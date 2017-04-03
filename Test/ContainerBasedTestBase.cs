using System.IO;
using Autofac;
using BankManager.Common;
using BankManager.Common.Model.Configuration;
using BankManager.Core.Configuration;
using BankManager.Core.Service;
using BankManager.Core.ValueProvider;
using BankManager.Test.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            //TODO revert before test 
            //ConfigurationModule.ConfigurationFilePath = TestConstants.Service.Configuration.Path;
            //File.Delete(TestConstants.Service.Configuration.Path);
            ContainerProvider = new TestContainerProvider();
            var keePassConfiguration = Container.Resolve<KeePassConfiguration>();
            keePassConfiguration.Path = TestConstants.Service.KeePass.Path;

            var keePassPasswordValueProvider = Container.Resolve<IKeePassPasswordValueProvider>();
            keePassPasswordValueProvider.RegisterPassword(TestConstants.Service.KeePass.Password);

            var configurationService = Container.Resolve<IConfigurationService>();
            configurationService.ApplicationConfiguration.DatabaseConfiguration.DatabasePath =
                TestConstants.Data.DatabasePath;

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