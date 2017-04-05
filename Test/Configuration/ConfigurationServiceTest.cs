using System.IO;
using Autofac;
using BankManager.Common;
using BankManager.Core.Provider;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BankManager.Test.Configuration
{
    [TestClass]
    public class ConfigurationServiceTest : ContainerBasedTestBase
    {
        public IConfigurationProvider ConfigurationProvider { get; set; }
        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();
            ConfigurationProvider = Container.Resolve<IConfigurationProvider>();
        }

        [TestMethod]
        public void TestExportConfiguration()
        {
            if (File.Exists(TestConstants.Service.Configuration.Path))
            {
                File.Delete(TestConstants.Service.Configuration.Path);
            }
            Assert.IsNotNull(ConfigurationProvider.ApplicationConfiguration);
            using (var stream = File.OpenWrite(TestConstants.Service.Configuration.Path))
            {
                ConfigurationProvider.ExportConfiguration(stream);
            }
        }

        [TestMethod]
        public void TestImportConfiguration()
        {
            TestExportConfiguration();

            var before = ConfigurationProvider.ApplicationConfiguration;

            Assert.IsTrue(File.Exists(TestConstants.Service.Configuration.Path));
            using (var stream = File.OpenRead(TestConstants.Service.Configuration.Path))
            {
                ConfigurationProvider.ImportConfiguration(stream);
            }

            var after = ConfigurationProvider.ApplicationConfiguration;

            Assert.IsNotNull(after);
            Assert.AreEqual(before, after);
        }
    }
}
