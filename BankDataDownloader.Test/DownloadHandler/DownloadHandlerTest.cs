﻿using System.IO;
using System.Linq;
using System.Text;
using BankDataDownloader.Common.Properties;
using BankDataDownloader.Core.DownloadHandler;
using DataDownloader.Test.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace DataDownloader.Test.DownloadHandler
{
    public abstract class DownloadHandlerTestBase<TDownloadHandler> where TDownloadHandler : BankDownloadHandlerBase
    {
        protected TDownloadHandler DownloadHandler;
        protected TestSettings TestSettings;

        [TestInitialize]
        public void TestInitialize()
        {
            var name = "DataDownloader.Test.testsettings.json";
            using (var stream = typeof(TestSettings).Assembly.GetManifestResourceStream(name))
            {
                if (stream != null)
                {
                    using (var sr = new StreamReader(stream, Encoding.Default))
                    {
                        TestSettings = JsonConvert.DeserializeObject<TestSettings>(sr.ReadToEnd());
                        SettingsHandler.RegisterSettingHandler(TestSettings);
                    }
                }
            }
            var constructors = typeof(TDownloadHandler).GetConstructors();
            var constructor = constructors.Single(info => info.GetParameters().First().ParameterType == typeof(string));
            DownloadHandler = (TDownloadHandler)constructor.Invoke(new object[] { TestSettings.KeePassPassword });
        }

        [TestMethod]
        public void TestDownloadAllData()
        {
            DownloadHandler.DownloadAllData();
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
    }

    [TestClass]
    public class Number26DownloadHandlerTest : DownloadHandlerTestBase<Number26DownloadHandler>
    {
    }

    [TestClass]
    public class RaiffeisenDownloadHandlerTest : DownloadHandlerTestBase<RaiffeisenDownloadHandler>
    {
    }

    [TestClass]
    public class SantanderDownloadHandlerTest : DownloadHandlerTestBase<SantanderDownloadHandler>
    {
    }

    [TestClass]
    public class RciDownloadHandlerTest : DownloadHandlerTestBase<RciDownloadHandler>
    {
    }
}
