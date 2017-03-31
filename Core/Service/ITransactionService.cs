using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BankDataDownloader.Core.Model.Transaction;

namespace BankDataDownloader.Core.Service
{
    public interface ITransactionService
    {
        IEnumerable<CumulativeTransactionModel> CumulativeAccountTransactions();
        IEnumerable<AggregatedTransactionModel> MonthlyAggregatedAccountCapital();
        IEnumerable<CumulativePositionModel> CumulativePortfolioPosition();
        IEnumerable<AggregatedTransactionModel> MontlyAggregatedPortfolioCapital();
        IEnumerable<BankTransactionModel> BankTransactions(long id);
        IEnumerable<BankTransactionForeignCurrencyModel> CreditCardTransactions(long id);
        IEnumerable<PortfolioPositionModel> PortfolioPositions(long id);
        PortfolioPositionModel CreatePortfolioSalePosition(PortfolioPositionModel model);
    }
}
