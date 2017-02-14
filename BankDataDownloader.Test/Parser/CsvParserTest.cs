using System;
using System.Linq;
using Autofac;
using BankDataDownloader.Core.Parser.Impl;
using BankDataDownloader.Data.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace DataDownloader.Test.Parser
{
    [TestClass]
    public class CsvParserTest : ContainerBasedTestBase
    {
        public CsvParser CsvParser { get; set; }
        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();
            CsvParser = Container.Resolve<CsvParser>();
        }

        [TestMethod]
        public void TestParse()
        {
            var results = CsvParser.Parse(TestConstants.Parser.CsvParser.RaiffeisenPath).ToList();
            IsNotNull(results);
            AreNotEqual(0, results.Count);
            foreach (var raiffeisenTransactionEntity in results)
            {
                Console.WriteLine(raiffeisenTransactionEntity);
            }
        }
    }
}
