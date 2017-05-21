using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BankManager.Core.Extension;
using BankManager.Core.Model.Transaction;
using BankManager.Data.Entity;
using BankManager.Data.Repository;
using Microsoft.EntityFrameworkCore;

namespace BankManager.Core.Service.Impl
{
    public class TransactionService : ITransactionService
    {
        public IBankTransactionRepository<TransactionEntity> BankTransactionRepository { get; }
        public IBankTransactionRepository<TransactionForeignCurrencyEntity> BankTransactionForeignCurrencyRepository { get; }
        public IPortfolioPositionRepository PortfolioPositionRepository { get; }
        public IPortfolioRepository PortfolioRepository { get; }
        public IMapper Mapper { get; }

        public TransactionService(IBankTransactionRepository<TransactionEntity> bankTransactionRepository, IBankTransactionRepository<TransactionForeignCurrencyEntity> bankTransactionForeignCurrencyRepository, IPortfolioPositionRepository portfolioPositionRepository, IMapper mapper, IPortfolioRepository portfolioRepository)
        {
            BankTransactionRepository = bankTransactionRepository;
            BankTransactionForeignCurrencyRepository = bankTransactionForeignCurrencyRepository;
            PortfolioPositionRepository = portfolioPositionRepository;
            Mapper = mapper;
            PortfolioRepository = portfolioRepository;
        }

        public IEnumerable<CumulativeTransactionModel> CumulativeAccountTransactions()
        {
            var transactions =
                BankTransactionRepository.Query().Include(entity => entity.Account).OrderBy(entity => entity.AvailabilityDate);
            var transactionModels = Mapper.Map<IEnumerable<CumulativeTransactionModel>>(transactions).ToList();
            var transSum = 0m;
            foreach (var transaction in transactionModels)
            {
                transSum += transaction.Amount;
                transaction.Cumulative = transSum;
            }

            transactionModels.Reverse();
            return transactionModels;
        }

        public IEnumerable<AggregatedTransactionModel> MonthlyAggregatedAccountCapital()
        {
            var aggregated = CumulativeAccountTransactions().GroupBy(entity => new { entity.AvailabilityDate.Year, entity.AvailabilityDate.Month }).Select(g => new AggregatedTransactionModel
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                Average = g.Average(model => model.Cumulative),
                Median = g.Median(model => model.Cumulative),
                StdDev = g.StdDev(model => model.Cumulative)
            });
            return aggregated;
        }

        public IEnumerable<CumulativePositionModel> CumulativePortfolioPosition()
        {
            var transactions = Mapper.Map<IEnumerable<CumulativePositionModel>>(
                    PortfolioPositionRepository.Query().Include(entity => entity.Portfolio))
                .OrderBy(e => e.DateTime).ToList();

            foreach (var group in transactions.GroupBy(entity => new { entity.PortfolioId, entity.Isin }))
            {
                var previousAmount = 0m;
                var previousValue = 0m;
                foreach (var cumulativePositionModel in group)
                {
                    cumulativePositionModel.ValuePerItemChange = cumulativePositionModel.CurrentValue - previousValue;
                    cumulativePositionModel.AmountChange = cumulativePositionModel.Amount - previousAmount;
                    previousAmount = cumulativePositionModel.Amount;
                    previousValue = cumulativePositionModel.CurrentValue;
                }
            }
            var transSum = 0m;
            foreach (var transaction in transactions)
            {
                transSum += transaction.CurrentValue * transaction.Amount - (transaction.CurrentValue - transaction.ValuePerItemChange) * (transaction.Amount - transaction.AmountChange);
                transaction.Cumulative = transSum;
            }
            transactions.Reverse();
            return transactions;
        }

        public IEnumerable<AggregatedTransactionModel> MontlyAggregatedPortfolioCapital()
        {
            var cumulative = CumulativePortfolioPosition().ToList();
            var minDate = cumulative.Min(model => model.DateTime);
            var maxDate = cumulative.Max(model => model.DateTime);
            var aggregated = cumulative.GroupBy(entity => new { entity.DateTime.Year, entity.DateTime.Month }).Select(g => new AggregatedTransactionModel
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                Average = g.Average(model => model.Cumulative),
                Median = g.Median(model => model.Cumulative),
                StdDev = g.StdDev(model => model.Cumulative)
            }).ToList();
            var lookup = aggregated.ToDictionary(model => new DateTime(model.Year, model.Month, 1));
            for (var date = minDate; date < maxDate; date = date.AddMonths(1))
            {
                var lookupKey = new DateTime(date.Year, date.Month, 1);
                if (!lookup.ContainsKey(lookupKey))
                {
                    var model = lookup[lookupKey.AddMonths(-1)];
                    lookup[lookupKey] = model;
                    aggregated.Add(new AggregatedTransactionModel
                    {
                        Year = lookupKey.Year,
                        Month = lookupKey.Month,
                        Average = model.Average,
                        Median = model.Median,
                        StdDev = model.StdDev
                    });
                }
            }
            return aggregated;
        }

        public IEnumerable<BankTransactionModel> GetBankTransactionsForAccountId(long id)
        {
            var transactions = BankTransactionRepository.GetAllForAccountId(id).OrderByDescending(entity => entity.AvailabilityDate);
            return Mapper.Map<IEnumerable<BankTransactionModel>>(transactions);
        }

        public IEnumerable<BankTransactionForeignCurrencyModel> GetCreditCardTransactionsForAccountId(long id)
        {
            var transactions = BankTransactionForeignCurrencyRepository.GetAllForAccountId(id).OrderByDescending(entity => entity.AvailabilityDate);
            return Mapper.Map<IEnumerable<BankTransactionForeignCurrencyModel>>(transactions);
        }

        public IEnumerable<PortfolioPositionModel> GetPortfolioPositionsForPortfolioId(long id)
        {
            var positions =
                PortfolioPositionRepository.GetAllByPortfolioId(id)
                    .GroupBy(entity => entity.Isin)
                    .Select(entities => entities.OrderByDescending(entity => entity.DateTime).FirstOrDefault()).ToList();
            return Mapper.Map<IEnumerable<PortfolioPositionModel>>(positions);
        }

        public IEnumerable<PortfolioPositionModel> GetAllPortfolioPositions()
        {
            var positions =
                PortfolioPositionRepository.Query()
                    .Include(entity => entity.Portfolio)
                    .GroupBy(entity => new { entity.Isin, entity.Portfolio })
                    .Select(entities => entities.OrderByDescending(entity => entity.DateTime).FirstOrDefault())
                    .ToList();
            return Mapper.Map<IEnumerable<PortfolioPositionModel>>(positions);
        }

        public BankTransactionModel DeleteBankTransaction(long id)
        {
            var transaction = BankTransactionRepository.GetById(id);
            BankTransactionRepository.Delete(transaction);
            BankTransactionRepository.Save();
            return Mapper.Map<BankTransactionModel>(transaction);
        }

        public BankTransactionForeignCurrencyModel DeleteCreditCardTransaction(long id)
        {
            var transaction = BankTransactionForeignCurrencyRepository.GetById(id);
            BankTransactionForeignCurrencyRepository.Delete(transaction);
            BankTransactionForeignCurrencyRepository.Save();
            return Mapper.Map<BankTransactionForeignCurrencyModel>(transaction);
        }

        public PortfolioPositionModel DeletePortfolioPosition(long id)
        {
            var transaction = PortfolioPositionRepository.GetById(id);
            PortfolioPositionRepository.Delete(transaction);
            PortfolioPositionRepository.Save();
            return Mapper.Map<PortfolioPositionModel>(transaction);
        }

        public PortfolioPositionModel CreatePortfolioSalePosition(PortfolioPositionModel model)
        {
            var portfolio = PortfolioRepository.GetById(model.PortfolioId);
            var otherPosOfIsin =
                PortfolioPositionRepository
                    .Query()
                    .First(positionEntity => positionEntity.Isin == model.Isin && positionEntity.Name != null);
            var entity = new SalePositionEntity
            {
                Amount = 0,
                DateTime = model.DateTime,
                Isin = model.Isin,
                Name = otherPosOfIsin.Name,
                CurrentValueCurrencyIso = otherPosOfIsin.CurrentValueCurrencyIso,
                OriginalValueCurrencyIso = otherPosOfIsin.OriginalValueCurrencyIso,
                Portfolio = portfolio
            };
            var insertedEntity = PortfolioPositionRepository.Insert(entity);
            PortfolioPositionRepository.Save();
            return Mapper.Map<PortfolioPositionModel>(insertedEntity);
        }
    }
}