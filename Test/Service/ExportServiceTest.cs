using System.IO;
using Autofac;
using BankManager.Core.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestConfiguration = BankManager.Test._Configuration.TestConfiguration;

namespace BankManager.Test.Service
{
    [TestClass]
    public class ExportServiceTest : ContainerBasedTestBase
    {
        public IExportService ExportService { get; set; }
        [TestInitialize]
        public void TestInitialize()
        {
            base.TestInitialize(useTestDatabase: true);
            ExportService = Container.Resolve<IExportService>();
        }
        [TestMethod]
        public void TestExport()
        {
            byte[] data;
            File.Delete(TestConfiguration.Export.Path);
            using (var fileWriter = File.OpenWrite(TestConfiguration.Export.Path))
            {
                data = ExportService.ExportAllToExcel();
                fileWriter.Write(data, 0, data.Length);
            }
            Assert.IsTrue(File.Exists(TestConfiguration.Export.Path));
            Assert.AreEqual(data.Length, new FileInfo(TestConfiguration.Export.Path).Length);
        }
        [TestCleanup]
        public override void TestCleanup()
        {
            base.TestCleanup();
        }
    }
}
