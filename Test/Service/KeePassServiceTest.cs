using BankManager.Common;
using BankManager.Common.Model.Configuration;
using BankManager.Core.Provider;
using BankManager.Core.Provider.Impl;
using BankManager.Core.Service;
using BankManager.Core.Service.Impl;
using BankManager.Test._Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestConfiguration = BankManager.Test._Configuration.TestConfiguration;

namespace BankManager.Test.Service
{
    [TestClass]
    public class KeePassServiceTest
    {
        public KeePassConfiguration KeePassConfiguration { get; set; }
        public IKeePassPasswordProvider KeePassPasswordProvider { get; set; }
        public IKeePassService KeePassService { get; set; }
        [TestInitialize]
        public void TestInitialize()
        {
            KeePassConfiguration = new KeePassConfiguration
            {
                Path = TestConfiguration.KeePass.Path,
            };
            KeePassPasswordProvider = new KeePassPasswordProvider();
            KeePassService = new KeePassService(KeePassConfiguration, KeePassPasswordProvider);
            KeePassPasswordProvider.RegisterPassword(TestConfiguration.KeePass.Password);
        }
        [TestMethod]
        public void TestKeePassServiceInit()
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
