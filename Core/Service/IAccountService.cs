using System.Collections.Generic;
using BankManager.Core.Model.Account;

namespace BankManager.Core.Service
{
    public interface IAccountService
    {
        IEnumerable<BankAccountModel> BankAccounts();
        IEnumerable<CreditCardAccountModel> CreditCards();
        IEnumerable<PortfolioModel> Portfolios();
        BankAccountModel BankAccount(long id);
        CreditCardAccountModel CreditCard(long id);
        PortfolioModel Portfolio(long id);
        BankAccountModel CreateEditBankAccount(BankAccountModel model);
        CreditCardAccountModel CreateEditCreditCard(CreditCardAccountModel model);
        PortfolioModel CreateEditPortfolio(PortfolioModel model);
    }
}
