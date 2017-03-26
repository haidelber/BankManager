using System;
using Autofac;
using BankDataDownloader.Core.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BankDataDownloader.Test.Service
{
    [TestClass]
    public class TransactionServiceTest : ContainerBasedTestBase
    {
        public ITransactionService TransactionService { get; set; }
        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();

            TransactionService = Container.Resolve<ITransactionService>();
        }

        [TestMethod]
        public void TestMethod1()
        {
            TransactionService.CumulativeAccountTransactions();
            TransactionService.MonthlyAggregatedAccountCapital();
            TransactionService.CumulativePortfolioPosition();
            TransactionService.MontlyAggregatedPortfolioCapital();
        }
    }
}
