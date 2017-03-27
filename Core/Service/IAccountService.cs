using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankDataDownloader.Core.Model.Account;
using BankDataDownloader.Data.Repository.Impl;

namespace BankDataDownloader.Core.Service
{
    public interface IAccountService
    {
        IEnumerable<BankAccountModel> BankAccounts();
        IEnumerable<CreditCardAccountModel> CreditCards();
        IEnumerable<PortfolioModel> Portfolios();
        BankAccountModel BankAccount(long id);
        CreditCardAccountModel CreditCard(long id);
        PortfolioModel Portfolio(long id);
    }
}
