using System;
using System.Linq;
using Autofac;
using BankDataDownloader.Common;
using BankDataDownloader.Core.Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BankDataDownloader.Test.Parser
{
    [TestClass]
    public class ExcelParserTest : ContainerBasedTestBase
    {
        public IFileParser CsvParser { get; set; }
        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();
            CsvParser = Container.ResolveKeyed<IFileParser>(Constants.UniqueContainerKeys.FileParserFlatexGiro);
        }

        [TestMethod]
        public void TestParse()
        {
            var results = CsvParser.Parse(TestConstants.Parser.CsvParser.FlatexGiroPath).ToList();
            Assert.IsNotNull(results);
            Assert.AreNotEqual(0, results.Count);
            foreach (var transactions in results)
            {
                Console.WriteLine(transactions);
            }
        }
    }
}
