using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using BankDataDownloader.Core.Extension;
using BankDataDownloader.Core.Model.Transaction;
using BankDataDownloader.Data.Entity;
using BankDataDownloader.Data.Repository;

namespace BankDataDownloader.Core.Service.Impl
{
    public class TransactionService : ITransactionService
    {
        public IBankTransactionRepository BankTransactionRepository { get; }
        public IBankTransactionRepository BankTransactionForeignCurrencyRepository { get; }
        public IPortfolioPositionRepository PortfolioPositionRepository { get; }
        public IPortfolioRepository PortfolioRepository { get; }
        public IMapper Mapper { get; }

        public TransactionService(IBankTransactionRepository bankTransactionRepository, IBankTransactionRepository bankTransactionForeignCurrencyRepository, IPortfolioPositionRepository portfolioPositionRepository, IMapper mapper, IPortfolioRepository portfolioRepository)
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
                StdDev = g.Select(model => model.Cumulative).StdDev()
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
                var previousValue = 0m;
                foreach (var cumulativePositionModel in group)
                {
                    var value = cumulativePositionModel.CurrentValue * cumulativePositionModel.Amount;
                    cumulativePositionModel.ChangeToPrevious = value - previousValue;
                    previousValue = value;
                }
            }
            var transSum = 0m;
            foreach (var transaction in transactions)
            {
                transSum += transaction.ChangeToPrevious;
                transaction.Cumulative = transSum;
            }
            transactions.Reverse();
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
            var positions = PortfolioPositionRepository.GetAllByPortfolioId(id).ToList();
            if (positions.Count <= 0) return new List<PortfolioPositionModel>();
            var maxDate = positions.Max(entity => entity.DateTime.Date);
            var onlyCurrent =
                positions.Where(entity => entity.DateTime.Date.Equals(maxDate)).OrderByDescending(entity => entity.Isin);
            return Mapper.Map<IEnumerable<PortfolioPositionModel>>(onlyCurrent);
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
            var entity = new PortfolioSalePositionEntity
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