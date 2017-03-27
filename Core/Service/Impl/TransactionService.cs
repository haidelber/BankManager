using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BankDataDownloader.Core.Extension;
using BankDataDownloader.Core.Model.Transaction;
using BankDataDownloader.Data.Repository;

namespace BankDataDownloader.Core.Service.Impl
{
    public class TransactionService : ITransactionService
    {
        public IBankTransactionRepository BankTransactionRepository { get; }
        public IBankTransactionRepository BankTransactionForeignCurrencyRepository { get; }
        public IPortfolioPositionRepository PortfolioPositionRepository { get; }
        public IMapper Mapper { get; }

        public TransactionService(IBankTransactionRepository bankTransactionRepository, IBankTransactionRepository bankTransactionForeignCurrencyRepository, IPortfolioPositionRepository portfolioPositionRepository, IMapper mapper)
        {
            BankTransactionRepository = bankTransactionRepository;
            BankTransactionForeignCurrencyRepository = bankTransactionForeignCurrencyRepository;
            PortfolioPositionRepository = portfolioPositionRepository;
            Mapper = mapper;
        }

        public IEnumerable<CumulativeTransactionModel> CumulativeAccountTransactions()
        {
            var transactions =
                BankTransactionRepository.GetAll().OrderBy(entity => entity.AvailabilityDate);
            var transactionModels = Mapper.Map<IEnumerable<CumulativeTransactionModel>>(transactions).ToList();
            var transSum = 0m;
            foreach (var transaction in transactionModels)
            {
                transSum += transaction.Amount;
                transaction.Cumulative = transSum;
            }

            return transactionModels;
        }

        public IEnumerable<AggregatedTransactionModel> MonthlyAggregatedAccountCapital()
        {
            var aggregated = CumulativeAccountTransactions().GroupBy(entity => new { entity.AvailabilityDate.Year, entity.AvailabilityDate.Month }).Select(g => new AggregatedTransactionModel
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                Average = g.Average(model => model.Cumulative),
                StdDev = g.Select(model => model.Cumulative).StdDev()
            });
            return aggregated;
        }

        public IEnumerable<CumulativePositionModel> CumulativePortfolioPosition()
        {
            var transactions = Mapper.Map<IEnumerable<CumulativePositionModel>>(
                    PortfolioPositionRepository.GetAll())
                .OrderBy(e => e.DateTime).ToList();

            foreach (var group in transactions.GroupBy(entity => new { entity.PortfolioId, entity.Isin }))
            {
                var previousValue = 0m;
                foreach (var cumulativePositionModel in group)
                {
                    cumulativePositionModel.ChangeToPrevious = cumulativePositionModel.CurrentValue * cumulativePositionModel.Amount - previousValue;
                    previousValue = cumulativePositionModel.CurrentValue;
                }
            }
            var transSum = 0m;
            foreach (var transaction in transactions)
            {
                transSum += transaction.ChangeToPrevious;
                transaction.Cumulative = transSum;
            }
            return transactions;
        }

        public IEnumerable<AggregatedTransactionModel> MontlyAggregatedPortfolioCapital()
        {
            var aggregated = CumulativePortfolioPosition().GroupBy(entity => new { entity.DateTime.Year, entity.DateTime.Month }).Select(g => new AggregatedTransactionModel
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                Average = g.Average(model => model.Cumulative),
                StdDev = g.Select(model => model.Cumulative).StdDev()
            });
            return aggregated;
        }

        public IEnumerable<BankTransactionModel> BankTransactions(long id)
        {
            var transactions = BankTransactionRepository.GetAllForAccountId(id).OrderByDescending(entity => entity.AvailabilityDate);
            return Mapper.Map<IEnumerable<BankTransactionModel>>(transactions);
        }

        public IEnumerable<BankTransactionForeignCurrencyModel> CreditCardTransactions(long id)
        {
            var transactions = BankTransactionForeignCurrencyRepository.GetAllForAccountId(id).OrderByDescending(entity => entity.AvailabilityDate);
            return Mapper.Map<IEnumerable<BankTransactionForeignCurrencyModel>>(transactions);
        }

        public IEnumerable<PortfolioPositionModel> PortfolioPositions(long id)
        {
            var positions = PortfolioPositionRepository.GetAllByPortfolioId(id).ToList();
            if (positions.Count > 0)
            {
                var maxDate = positions.Max(entity => entity.DateTime.Date);
                var onlyCurrent =
                    positions.Where(entity => entity.DateTime.Date.Equals(maxDate)).OrderByDescending(entity => entity.Isin);
                return Mapper.Map<IEnumerable<PortfolioPositionModel>>(onlyCurrent);
            }
            return new List<PortfolioPositionModel>();
        }
    }
}