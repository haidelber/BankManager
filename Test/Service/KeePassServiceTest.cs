﻿using BankManager.Common;
using BankManager.Common.Model.Configuration;
using BankManager.Core.Service;
using BankManager.Core.Service.Impl;
using BankManager.Core.ValueProvider;
using BankManager.Core.ValueProvider.Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BankManager.Test.Service
{
    [TestClass]
    public class KeePassServiceTest
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
