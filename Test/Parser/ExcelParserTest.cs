using System;
using System.Linq;
using Autofac;
using BankManager.Common;
using BankManager.Core.Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestConfiguration = BankManager.Test._Configuration.TestConfiguration;

namespace BankManager.Test.Parser
{
    [TestClass]
    public class ExcelParserTest : ContainerBasedTestBase
    {
        public IFileParser ExcelParser { get; set; }
        [TestInitialize]
        public void TestInitialize()
        {
            base.TestInitialize();
            ExcelParser = Container.ResolveKeyed<IFileParser>(Constants.UniqueContainerKeys.FileParserFlatexGiro);
        }

        [TestMethod]
        public void TestExcelParserParse()
        {
            var results = ExcelParser.Parse(TestConfiguration.Parser.FlatexGiroPath).ToList();
            Assert.IsNotNull(results);
            Assert.AreNotEqual(0, results.Count);
            foreach (var transactions in results)
            {
                Console.WriteLine(transactions);
            }
        }
    }
}
