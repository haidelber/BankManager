using System;
using System.IO;
using System.Linq;
using BankDataDownloader.Common.Model.Configuration;
using BankDataDownloader.Core.Service;
using BankDataDownloader.Core.Service.Impl;
using BankDataDownloader.Data;
using DataDownloader.Test.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataDownloader.Test.Data
{
    [TestClass]
    public class BasicDataTest : DataTestBase
    {
        [TestMethod]
        public void TestAvailabilityOfSQLite()
        {
            var trans = DataContext.RaiffeisenTransactions.ToList();
        }
    }
}
