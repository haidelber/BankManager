using System.Linq;
using BankManager.Core.Service;
using BankManager.Core.Service.Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLog;

namespace BankManager.Test.Service
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
        public void TestLogServiceInit()
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
