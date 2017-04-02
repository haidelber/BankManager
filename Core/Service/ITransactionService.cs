using System.Collections.Generic;
using BankManager.Core.Model.Transaction;

namespace BankManager.Core.Service
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
