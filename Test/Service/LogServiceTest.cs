using System.Linq;
using BankDataDownloader.Common;
using BankDataDownloader.Common.Model.Configuration;
using BankDataDownloader.Core.Service;
using BankDataDownloader.Core.Service.Impl;
using BankDataDownloader.Core.ValueProvider;
using BankDataDownloader.Core.ValueProvider.Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLog;

namespace BankDataDownloader.Test.Service
{
    [TestClass]
    public class LogServiceTest
    {
        public ILogService LogService { get; set; }
        public readonly Logger Logger = LogManager.GetCurrentClassLogger();
        [TestInitialize]
        public void TestInitialize()
        {
            LogService = new LogService();
        }
        [TestMethod]
        public void TestServiceInit()
        {
            for (int i = 0; i < 100; i++)
            {
                Logger.Debug(i);
            }
            Assert.IsTrue(LogService.GetLogFilePaths().Any());
        }
        [TestCleanup]
        public void TestCleanup()
        {

        }
    }
}
