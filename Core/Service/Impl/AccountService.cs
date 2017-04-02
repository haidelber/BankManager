using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BankManager.Core.Model.Account;
using BankManager.Data.Entity;
using BankManager.Data.Repository;

namespace BankManager.Core.Service.Impl
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

        public BankAccountModel CreateEditBankAccount(BankAccountModel model)
        {
            BankAccountEntity entity;
            //is update
            if (model.Id != default(long))
            {
                entity = BankAccountRepository.GetById(model.Id);
                entity = Mapper.Map(model, entity);
            }
            else
            {
                entity = Mapper.Map<BankAccountEntity>(model);
                entity = BankAccountRepository.Insert(entity);
            }
            BankAccountRepository.Save();
            return Mapper.Map<BankAccountModel>(entity);
        }

        public CreditCardAccountModel CreateEditCreditCard(CreditCardAccountModel model)
        {
            CreditCardEntity entity;
            //is update
            if (model.Id != default(long))
            {
                entity = CreditCardAccountRepository.GetById(model.Id);
                entity = Mapper.Map(model, entity);
            }
            else
            {
                entity = Mapper.Map<CreditCardEntity>(model);
                entity = CreditCardAccountRepository.Insert(entity);
            }
            CreditCardAccountRepository.Save();
            return Mapper.Map<CreditCardAccountModel>(entity);
        }

        public PortfolioModel CreateEditPortfolio(PortfolioModel model)
        {
            PortfolioEntity entity;
            //is update
            if (model.Id != default(long))
            {
                entity = PortfolioRepository.GetById(model.Id);
                entity = Mapper.Map(model, entity);
            }
            else
            {
                entity = Mapper.Map<PortfolioEntity>(model);
                entity = PortfolioRepository.Insert(entity);
            }
            PortfolioRepository.Save();
            return Mapper.Map<PortfolioModel>(entity);
        }
    }
}