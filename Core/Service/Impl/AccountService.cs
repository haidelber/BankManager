using System.Collections.Generic;
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
            return Mapper.Map<IEnumerable<BankAccountModel>>(BankAccountRepository.GetAll());
        }

        public IEnumerable<CreditCardAccountModel> CreditCards()
        {
            return Mapper.Map<IEnumerable<CreditCardAccountModel>>(CreditCardAccountRepository.GetAll());
        }

        public IEnumerable<PortfolioModel> Portfolios()
        {
            return Mapper.Map<IEnumerable<PortfolioModel>>(PortfolioRepository.GetAll());
        }
    }
}