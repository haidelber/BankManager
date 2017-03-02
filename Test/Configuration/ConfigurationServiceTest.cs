using System.IO;
using Autofac;
using BankDataDownloader.Common;
using BankDataDownloader.Core.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BankDataDownloader.Test.Configuration
{
    [TestClass]
    public class ConfigurationServiceTest : ContainerBasedTestBase
    {
        public IConfigurationService ConfigurationService { get; set; }
        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();
            ConfigurationService = Container.Resolve<IConfigurationService>();
        }

        [TestMethod]
        public void TestExportConfiguration()
        {
            if (File.Exists(TestConstants.Service.Configuration.Path))
            {
                File.Delete(TestConstants.Service.Configuration.Path);
            }
            Assert.IsNotNull(ConfigurationService.ApplicationConfiguration);
            using (var stream = File.OpenWrite(TestConstants.Service.Configuration.Path))
            {
                ConfigurationService.ExportConfiguration(stream);
            }
        }

        [TestMethod]
        public void TestImportConfiguration()
        {
            TestExportConfiguration();
            Assert.IsTrue(File.Exists(TestConstants.Service.Configuration.Path));
            using (var stream = File.OpenRead(TestConstants.Service.Configuration.Path))
            {
                ConfigurationService.ImportConfiguration(stream);
            }
            Assert.IsNotNull(ConfigurationService.ApplicationConfiguration);
            //TODO asserts based on DefaultConfiguration
        }
    }
}
