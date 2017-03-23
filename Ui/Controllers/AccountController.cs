using System.Collections.Generic;
using AutoMapper;
using BankDataDownloader.Data.Repository;
using BankManager.Ui.Model.Account;
using Microsoft.AspNetCore.Mvc;

namespace BankManager.Ui.Controllers
{
    public class AccountController : ApiController
    {
        public IBankAccountRepository BankAccountRepository { get; }
        public ICreditCardAccountRepository CreditCardAccountRepository { get; }
        public IPortfolioRepository PortfolioRepository { get; }
        public IMapper Mapper { get; }

        public AccountController(IBankAccountRepository bankAccountRepository, ICreditCardAccountRepository creditCardAccountRepository, IPortfolioRepository portfolioRepository, IMapper mapper)
        {
            BankAccountRepository = bankAccountRepository;
            CreditCardAccountRepository = creditCardAccountRepository;
            PortfolioRepository = portfolioRepository;
            Mapper = mapper;
        }

        [HttpGet("BankAccount")]
        public IActionResult GetBank()
        {
            return Json(GetBankAccountModel());
        }

        [HttpGet("CreditCard")]
        public IActionResult GetCreditCard()
        {
            return Json(GetCreditCardModel());
        }

        [HttpGet("Portfolio")]
        public IActionResult GetPortfolio()
        {
            return Json(GetPortfolioModel());
        }

        private IEnumerable<BankAccountModel> GetBankAccountModel()
        {
            return Mapper.Map<IEnumerable<BankAccountModel>>(BankAccountRepository.GetAll());
        }

        private IEnumerable<CreditCardAccountModel> GetCreditCardModel()
        {
            return Mapper.Map<IEnumerable<CreditCardAccountModel>>(CreditCardAccountRepository.GetAll());
        }

        private IEnumerable<PortfolioModel> GetPortfolioModel()
        {
            return Mapper.Map<IEnumerable<PortfolioModel>>(PortfolioRepository.GetAll());
        }
    }
}