using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BankDataDownloader.Data.Entity;
using BankDataDownloader.Data.Repository;
using BankManager.Ui.Model.Transaction;
using Microsoft.AspNetCore.Mvc;

namespace BankManager.Ui.Controllers
{
    public class TransactionController : ApiController
    {
        public IRepository<BankTransactionEntity> TransactionRepository { get; }
        public IRepository<BankTransactionForeignCurrencyEntity> TransactionForeignCurrencyRepository { get; }
        public IRepository<PortfolioPositionEntity> PortfolioPositionRepository { get; }
        public IMapper Mapper { get; }

        public TransactionController(IRepository<BankTransactionEntity> transactionRepository, IRepository<BankTransactionForeignCurrencyEntity> transactionForeignCurrencyRepository, IRepository<PortfolioPositionEntity> portfolioPositionRepository, IMapper mapper)
        {
            TransactionRepository = transactionRepository;
            TransactionForeignCurrencyRepository = transactionForeignCurrencyRepository;
            PortfolioPositionRepository = portfolioPositionRepository;
            Mapper = mapper;
        }

        [HttpGet("BankAccount/{id}")]
        public IActionResult GetBankTransaction(long id)
        {
            return Json(GetBankTransactionModel(id));
        }

        [HttpGet("CreditCard/{id}")]
        public IActionResult GetCreditCardTransaction(long id)
        {
            return Json(GetCreditCardTransactionModel(id));
        }

        [HttpGet("Portfolio/{id}")]
        public IActionResult GetPortfolioPosition(long id)
        {
            return Json(GetPortfolioPositionModel(id));
        }

        private IEnumerable<BankTransactionModel> GetBankTransactionModel(long id)
        {
            var transactions = TransactionRepository.Query().Where(entity => entity.Account.Id == id).OrderBy(entity => entity.AvailabilityDate);
            return Mapper.Map<IEnumerable<BankTransactionModel>>(transactions);
        }

        private IEnumerable<BankTransactionForeignCurrencyModel> GetCreditCardTransactionModel(long id)
        {
            var transactions = TransactionForeignCurrencyRepository.Query().Where(entity => entity.Account.Id == id).OrderBy(entity => entity.AvailabilityDate);
            return Mapper.Map<IEnumerable<BankTransactionForeignCurrencyModel>>(transactions);
        }

        private IEnumerable<PortfolioPositionModel> GetPortfolioPositionModel(long id)
        {
            var positions = PortfolioPositionRepository.Query().Where(entity => entity.Portfolio.Id == id).OrderBy(entity => entity.DateTime);
            return Mapper.Map<IEnumerable<PortfolioPositionModel>>(positions);
        }
    }
}