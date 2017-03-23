using System;
using System.Collections.Generic;
using AutoMapper;
using BankDataDownloader.Data.Repository;
using BankManager.Ui.Model.Transaction;
using Microsoft.AspNetCore.Mvc;

namespace BankManager.Ui.Controllers
{
    public class TransactionController : ApiController
    {
        public IBankAccountRepository BankAccountRepository { get; }
        public ICreditCardAccountRepository CreditCardAccountRepository { get; }
        public IPortfolioRepository PortfolioRepository { get; }
        public IMapper Mapper { get; }

        public TransactionController(IBankAccountRepository bankAccountRepository, ICreditCardAccountRepository creditCardAccountRepository, IPortfolioRepository portfolioRepository, IMapper mapper)
        {
            BankAccountRepository = bankAccountRepository;
            CreditCardAccountRepository = creditCardAccountRepository;
            PortfolioRepository = portfolioRepository;
            Mapper = mapper;
        }

        [HttpGet("BankAccount/{id}")]
        public IActionResult GetBankTransaction(Guid id)
        {
            return Json(GetBankTransactionModel(id));
        }

        [HttpGet("CreditCard/{id}")]
        public IActionResult GetCreditCardTransaction(Guid id)
        {
            return Json(GetCreditCardTransactionModel(id));
        }

        [HttpGet("Portfolio/{id}")]
        public IActionResult GetPortfolioPosition(Guid id)
        {
            return Json(GetPortfolioPositionModel(id));
        }

        private IEnumerable<BankTransactionModel> GetBankTransactionModel(Guid id)
        {
            return Mapper.Map<IEnumerable<BankTransactionModel>>(BankAccountRepository.GetById(id).Transactions);
        }

        private IEnumerable<BankTransactionModel> GetCreditCardTransactionModel(Guid id)
        {
            return Mapper.Map<IEnumerable<BankTransactionModel>>(CreditCardAccountRepository.GetById(id).Transactions);
        }

        private IEnumerable<PortfolioPositionModel> GetPortfolioPositionModel(Guid id)
        {
            return Mapper.Map<IEnumerable<PortfolioPositionModel>>(PortfolioRepository.GetById(id).Positions);
        }
    }
}