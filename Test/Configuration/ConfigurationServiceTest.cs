using System.IO;
using Autofac;
using BankManager.Core.Provider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using TestConfiguration = BankManager.Test._Configuration.TestConfiguration;

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
            if (File.Exists(TestConfiguration.Configuration.Path))
            {
                File.Delete(TestConfiguration.Configuration.Path);
            }
            IsNotNull(ConfigurationProvider.ApplicationConfiguration);
            using (var stream = File.OpenWrite(TestConfiguration.Configuration.Path))
            {
                ConfigurationProvider.ExportConfiguration(stream);
            }
        }

        [TestMethod]
        public void TestImportConfiguration()
        {
            TestExportConfiguration();

            var before = ConfigurationProvider.ApplicationConfiguration;

            IsTrue(File.Exists(TestConfiguration.Configuration.Path));
            using (var stream = File.OpenRead(TestConfiguration.Configuration.Path))
            {
                ConfigurationProvider.ImportConfiguration(stream);
            }

            var after = ConfigurationProvider.ApplicationConfiguration;

            IsNotNull(after);
            AreEqual(before, after);
        }
    }
}
