using System;

namespace BankDataDownloader.Data.Entity.BankTransactions
{
    public class RaiffeisenPortfolioPositionEntity : PortfolioPositionEntity,
        IEntityEqualityComparer<RaiffeisenPortfolioPositionEntity>
    {
        public Func<RaiffeisenPortfolioPositionEntity, bool> Func(RaiffeisenPortfolioPositionEntity otherEntity)
        {
            return
                entity =>
                    entity.DateTime == otherEntity.DateTime && entity.Isin == otherEntity.Isin &&
                    entity.Amount == otherEntity.Amount;
        }
    }
}