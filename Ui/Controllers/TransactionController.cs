using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BankDataDownloader.Core.Extension;
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

        [HttpGet("Cumulative/Account")]
        public IActionResult GetCumulativeAccount()
        {
            return Json(GetCumulative());
        }

        [HttpGet("Aggregated/Account")]
        public IActionResult GetAggregatedMonthlyAccount()
        {
            return Json(GetAggregatedMonthly());
        }

        [HttpGet("Cumulative/Portfolio")]
        public IActionResult GetCumulativePortfolio()
        {
            return Json(GetCumulativePortfolioModel());
        }

        [HttpGet("Aggregated/Portfolio")]
        public IActionResult GetAggregatedMonthlyPortfolio()
        {
            return Json(GetAggregatedMonthlyPortfolioModel());
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
        public IActionResult GetCurrentPortfolioPosition(long id)
        {
            return Json(GetPortfolioPositionModel(id));
        }

        private IEnumerable<CumulativeTransactionModel> GetCumulative()
        {
            var transactions =
                TransactionRepository.GetAll().OrderBy(entity => entity.AvailabilityDate);
            var transactionModels = Mapper.Map<IEnumerable<CumulativeTransactionModel>>(transactions).ToList();
            var transSum = 0m;
            foreach (var transaction in transactionModels)
            {
                transSum += transaction.Amount;
                transaction.Cumulative = transSum;
            }

            return transactionModels;
        }

        private IEnumerable<AggregatedTransactionModel> GetAggregatedMonthly()
        {
            var aggregated = GetCumulative().GroupBy(entity => new { entity.AvailabilityDate.Year, entity.AvailabilityDate.Month }).Select(g => new AggregatedTransactionModel
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                Average = g.Average(model => model.Cumulative),
                StdDev = g.Select(model => model.Cumulative).StdDev()
            });
            return aggregated;
        }

        private IEnumerable<CumulativePositionModel> GetCumulativePortfolioModel()
        {
            var transactions = Mapper.Map<IEnumerable<CumulativePositionModel>>(
                PortfolioPositionRepository.GetAll())
                    .OrderBy(e => e.DateTime).ToList();

            foreach (var group in transactions.GroupBy(entity => new { entity.PortfolioId, entity.Isin }))
            {
                var previousValue = 0m;
                foreach (var cumulativePositionModel in group)
                {
                    cumulativePositionModel.ChangeToPrevious = cumulativePositionModel.CurrentValue - previousValue;
                    previousValue = cumulativePositionModel.CurrentValue;
                }
            }
            var transSum = 0m;
            foreach (var transaction in transactions)
            {
                transSum += transaction.Amount;
                transaction.Cumulative = transSum;
            }
            return transactions;
        }

        private IEnumerable<AggregatedTransactionModel> GetAggregatedMonthlyPortfolioModel()
        {
            var aggregated = GetCumulativePortfolioModel().GroupBy(entity => new { entity.DateTime.Year, entity.DateTime.Month }).Select(g => new AggregatedTransactionModel
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                Average = g.Average(model => model.Cumulative),
                StdDev = g.Select(model => model.Cumulative).StdDev()
            });
            return aggregated;
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
            var positions = PortfolioPositionRepository.Query().Where(entity => entity.Portfolio.Id == id).ToList();
            var maxDate = positions.Max(entity => entity.DateTime.Date);
            var onlyCurrent = positions.Where(entity => entity.DateTime.Date.Equals(maxDate)).OrderBy(entity => entity.Isin);
            return Mapper.Map<IEnumerable<PortfolioPositionModel>>(positions);
        }
    }
}