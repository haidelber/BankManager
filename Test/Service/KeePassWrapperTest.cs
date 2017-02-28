using BankDataDownloader.Common;
using BankDataDownloader.Common.Model.Configuration;
using BankDataDownloader.Core.Service;
using BankDataDownloader.Core.Service.Impl;
using BankDataDownloader.Core.ValueProvider;
using BankDataDownloader.Core.ValueProvider.Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BankDataDownloader.Test.Service
{
    [TestClass]
    public class KeePassWrapperTest
    {
        public KeePassConfiguration KeePassConfiguration { get; set; }
        public IKeePassPasswordValueProvider KeePassPasswordValueProvider { get; set; }
        public IKeePassService KeePassService { get; set; }
        [TestInitialize]
        public void TestInitialize()
        {
            KeePassConfiguration = new KeePassConfiguration
            {
                Path = TestConstants.Service.KeePass.Path,
            };
            KeePassPasswordValueProvider = new KeePassPasswordValueProvider();
            KeePassService = new KeePassService(KeePassConfiguration, KeePassPasswordValueProvider);
            KeePassPasswordValueProvider.RegisterPassword(TestConstants.Service.KeePass.Password);
        }
        [TestMethod]
        public void TestServiceInit()
        {
            KeePassService.Open();
            var entry = KeePassService.GetEntryByTitle("");
        }
        [TestCleanup]
        public void TestCleanup()
        {
            KeePassService.Dispose();
        }
    }
}
