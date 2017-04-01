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
        IEnumerable<BankTransactionModel> GetBankTransactionsForAccountId(long id);
        IEnumerable<BankTransactionForeignCurrencyModel> GetCreditCardTransactionsForAccountId(long id);
        IEnumerable<PortfolioPositionModel> GetPortfolioPositionsForPortfolioId(long id);
        PortfolioPositionModel CreatePortfolioSalePosition(PortfolioPositionModel model);
        BankTransactionModel DeleteBankTransaction(long id);
        BankTransactionForeignCurrencyModel DeleteCreditCardTransaction(long id);
        PortfolioPositionModel DeletePortfolioPosition(long id);
    }
}
