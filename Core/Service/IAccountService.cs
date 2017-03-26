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
    }
}
