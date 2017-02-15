using System;
using System.IO;
using Autofac;
using BankDataDownloader.Core.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace DataDownloader.Test.Configuration
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
            IsNotNull(ConfigurationService.ApplicationConfiguration);
            using (var stream = File.OpenWrite(TestConstants.Service.Configuration.Path))
            {
                ConfigurationService.ExportConfiguration(stream);
            }
            IsTrue(File.Exists(TestConstants.Service.Configuration.Path));
            using (var stream = File.OpenRead(TestConstants.Service.Configuration.Path))
            {
                ConfigurationService.ImportConfiguration(stream);
            }
        }

        [TestMethod]
        public void TestImportConfiguration()
        {
            TestExportConfiguration();
            IsTrue(File.Exists(TestConstants.Service.Configuration.Path));
            using (var stream = File.OpenRead(TestConstants.Service.Configuration.Path))
            {
                ConfigurationService.ImportConfiguration(stream);
            }
        }
    }
}
