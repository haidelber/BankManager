using BankDataDownloader.Common.Model.Configuration;
using BankDataDownloader.Core.Service;
using BankDataDownloader.Core.Service.Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataDownloader.Test.Service
{
    [TestClass]
    public class KeePassWrapperTest
    {
        public KeePassConfiguration KeePassConfiguration { get; set; }
        public IKeePassService KeePassService { get; set; }
        [TestInitialize]
        public void TestInitialize()
        {
            KeePassConfiguration = new KeePassConfiguration
            {
                Path = TestConstants.Service.KeePass.Path,
                Password = TestConstants.Service.KeePass.Password
            };
            KeePassService = new KeePassService(KeePassConfiguration);
        }
        [TestMethod]
        public void TestServiceInit()
        {
            var entry = KeePassService.GetEntryByTitle("");
        }
        [TestCleanup]
        public void TestCleanup()
        {
            KeePassService.Dispose();
        }
    }
}
