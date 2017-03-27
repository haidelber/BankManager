using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BankDataDownloader.Core.Model.Account;
using BankDataDownloader.Data.Repository;

namespace BankDataDownloader.Core.Service.Impl
{
    public class AccountService : IAccountService
    {
        public IBankAccountRepository BankAccountRepository { get; }
        public ICreditCardAccountRepository CreditCardAccountRepository { get; }
        public IPortfolioRepository PortfolioRepository { get; }
        public IMapper Mapper { get; }

        public AccountService(IBankAccountRepository bankAccountRepository, ICreditCardAccountRepository creditCardAccountRepository, IPortfolioRepository portfolioRepository, IMapper mapper)
        {
            BankAccountRepository = bankAccountRepository;
            CreditCardAccountRepository = creditCardAccountRepository;
            PortfolioRepository = portfolioRepository;
            Mapper = mapper;
        }

        public IEnumerable<BankAccountModel> BankAccounts()
        {
            return Mapper.Map<IEnumerable<BankAccountModel>>(BankAccountRepository.GetAll().OrderBy(entity => entity.BankName).ThenBy(entity => entity.AccountName));
        }

        public IEnumerable<CreditCardAccountModel> CreditCards()
        {
            return Mapper.Map<IEnumerable<CreditCardAccountModel>>(CreditCardAccountRepository.GetAll().OrderBy(entity => entity.BankName).ThenBy(entity => entity.AccountName));
        }

        public IEnumerable<PortfolioModel> Portfolios()
        {
            return Mapper.Map<IEnumerable<PortfolioModel>>(PortfolioRepository.GetAll().OrderBy(entity => entity.BankName).ThenBy(entity => entity.AccountName));
        }

        public BankAccountModel BankAccount(long id)
        {
            return Mapper.Map<BankAccountModel>(BankAccountRepository.GetById(id));
        }

        public CreditCardAccountModel CreditCard(long id)
        {
            return Mapper.Map<CreditCardAccountModel>(CreditCardAccountRepository.GetById(id));
        }

        public PortfolioModel Portfolio(long id)
        {
            return Mapper.Map<PortfolioModel>(PortfolioRepository.GetById(id));
        }
    }
}