using Autofac;
using BankDataDownloader.Common;
using BankDataDownloader.Common.Model.Configuration;
using BankDataDownloader.Core.DownloadHandler.Impl;
using BankDataDownloader.Core.Service;
using BankDataDownloader.Core.ValueProvider;
using BankDataDownloader.Core.ValueProvider.Impl;
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
            var keePassConfiguration = Container.Resolve<KeePassConfiguration>();
            keePassConfiguration.Path = TestConstants.Service.KeePass.Path;

            var keePassPasswordValueProvider = Container.Resolve<IKeePassPasswordValueProvider>();
            keePassPasswordValueProvider.RegisterPassword(TestConstants.Service.KeePass.Password);

            var configurationService = Container.Resolve<IConfigurationService>();
            configurationService.ApplicationConfiguration.DatabaseConfiguration.DatabasePath =
                TestConstants.Data.DatabasePath;
        }

        [TestCleanup]
        public virtual void TestCleanup()
        {
            ContainerProvider.Dispose();
        }
    }
}