using System;
using System.Linq;
using Autofac;
using BankManager.Common;
using BankManager.Core.Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BankManager.Test.Parser
{
    [TestClass]
    public class CsvParserTest : ContainerBasedTestBase
    {
        public IFileParser CsvParser { get; set; }
        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();
            CsvParser = Container.ResolveKeyed<IFileParser>(Constants.UniqueContainerKeys.FileParserRaiffeisenGiro);
        }

        [TestMethod]
        public void TestCsvParserParse()
        {
            var results = CsvParser.Parse(TestConstants.Parser.CsvParser.RaiffeisenPath).ToList();
            Assert.IsNotNull(results);
            Assert.AreNotEqual(0, results.Count);
            foreach (var raiffeisenTransactionEntity in results)
            {
                Console.WriteLine(raiffeisenTransactionEntity);
            }
        }
    }
}
