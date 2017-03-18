using System;

namespace BankDataDownloader.Data.Entity.BankTransactions
{
    public class FlatexPortfolioPositionEntity : PortfolioPositionEntity,
        IEntityEqualityComparer<FlatexPortfolioPositionEntity>
    {
        public string Category { get; set; }
        public string Depository { get; set; }
        public string StockExchange { get; set; }

        public Func<FlatexPortfolioPositionEntity, bool> Func(FlatexPortfolioPositionEntity otherEntity)
        {
            return
                entity =>
                    entity.DateTime == otherEntity.DateTime && entity.Isin == otherEntity.Isin &&
                    entity.Amount == otherEntity.Amount;
        }
    }
}