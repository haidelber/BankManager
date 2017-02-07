using System.IO;
using System.Linq;
using System.Text;
using BankDataDownloader.Core.DownloadHandler;
using BankDataDownloader.Core.DownloadHandler.Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace DataDownloader.Test.DownloadHandler
{
    public abstract class DownloadHandlerTestBase<TDownloadHandler> where TDownloadHandler : BankDownloadHandlerBase
    {
        protected TDownloadHandler DownloadHandler;

        [TestInitialize]
        public abstract void TestInitialize();

        [TestMethod]
        public void TestDownloadAllData()
        {
            DownloadHandler.Execute();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            DownloadHandler.Dispose();
        }
    }

    [TestClass]
    public class DkbDownloadHandlerTest : DownloadHandlerTestBase<DkbDownloadHandler>
    {
        public override void TestInitialize()
        {

        }
    }

    [TestClass]
    public class Number26DownloadHandlerTest : DownloadHandlerTestBase<Number26DownloadHandler>
    {
        public override void TestInitialize()
        {
            throw new System.NotImplementedException();
        }
    }

    [TestClass]
    public class RaiffeisenDownloadHandlerTest : DownloadHandlerTestBase<RaiffeisenDownloadHandler>
    {
        public override void TestInitialize()
        {
            throw new System.NotImplementedException();
        }
    }

    [TestClass]
    public class SantanderDownloadHandlerTest : DownloadHandlerTestBase<SantanderDownloadHandler>
    {
        public override void TestInitialize()
        {
            throw new System.NotImplementedException();
        }
    }

    [TestClass]
    public class RciDownloadHandlerTest : DownloadHandlerTestBase<RciDownloadHandler>
    {
        public override void TestInitialize()
        {
            throw new System.NotImplementedException();
        }
    }
}
