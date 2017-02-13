using System;
using System.Text;
using System.Collections.Generic;
using Autofac;
using BankDataDownloader.Common.Model.Configuration;
using BankDataDownloader.Core.Service;
using BankDataDownloader.Core.Service.Impl;
using BankDataDownloader.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataDownloader.Test.Configuration
{
    [TestClass]
    public class AutofacTest
    {
        public TestContainerProvider ContainerProvider { get; set; }

        [TestInitialize]
        public virtual void TestInitialize()
        {
            ContainerProvider = new TestContainerProvider();
        }

        [TestMethod]
        public void TestConfigurationModule()
        {
            var config = ContainerProvider.Container.Resolve<IConfigurationService>();
            var appConfig = ContainerProvider.Container.Resolve<ApplicationConfiguration>();
            var dbConfig = ContainerProvider.Container.Resolve<DatabaseConfiguration>();
        }

        [TestMethod]
        public void TestParserModule()
        {

        }
    }
}
