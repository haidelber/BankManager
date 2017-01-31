using System;
using DataDownloader.Test.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace DataDownloader.Test.Other
{
    [TestClass]
    public class TestSettingHandlerTest
    {
        [TestMethod]
        public void WriteTestSettingSkeleton()
        {
            Console.WriteLine(JsonConvert.SerializeObject(new TestSettings(), Formatting.Indented));
        }
    }
}
