using Autofac;
using BankManager.Core.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BankManager.Test.Service
{
    [TestClass]
    public class TransactionServiceTest : ContainerBasedTestBase
    {
        public ITransactionService TransactionService { get; set; }
        [TestInitialize]
        public void TestInitialize()
        {
            base.TestInitialize();

            TransactionService = Container.Resolve<ITransactionService>();
        }

        [TestMethod]
        public void TestTransactionService()
        {
            TransactionService.CumulativeAccountTransactions();
            TransactionService.MonthlyAggregatedAccountCapital();
            TransactionService.CumulativePortfolioPosition();
            TransactionService.MontlyAggregatedPortfolioCapital();
        }
    }
}
