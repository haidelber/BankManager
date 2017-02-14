using System;
using System.Linq;
using Autofac;
using BankDataDownloader.Common;
using BankDataDownloader.Core.Parser;
using BankDataDownloader.Core.Parser.Impl;
using BankDataDownloader.Data.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace DataDownloader.Test.Parser
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
            IsNotNull(results);
            AreNotEqual(0, results.Count);
            foreach (var raiffeisenTransactionEntity in results)
            {
                Console.WriteLine(raiffeisenTransactionEntity);
            }
        }
    }
}
