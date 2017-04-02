using System.IO;
using Autofac;
using BankManager.Common;
using BankManager.Core.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BankManager.Test.Configuration
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

            var before = ConfigurationService.ApplicationConfiguration;

            Assert.IsTrue(File.Exists(TestConstants.Service.Configuration.Path));
            using (var stream = File.OpenRead(TestConstants.Service.Configuration.Path))
            {
                ConfigurationService.ImportConfiguration(stream);
            }

            var after = ConfigurationService.ApplicationConfiguration;

            Assert.IsNotNull(after);
            Assert.AreEqual(before, after);
        }
    }
}
