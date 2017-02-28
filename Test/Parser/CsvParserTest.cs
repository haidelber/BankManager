using System;
using System.Linq;
using Autofac;
using BankDataDownloader.Common;
using BankDataDownloader.Core.Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BankDataDownloader.Test.Parser
{
    [TestClass]
    public class CsvParserTest : ContainerBasedTestBase
    {
        public IFileParser CsvParser { get; set; }
        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();
            CsvParser = Container.ResolveNamed<IFileParser>(Constants.UniqueContainerKeys.FileParserRaiffeisen);
        }

        [TestMethod]
        public void TestParse()
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
