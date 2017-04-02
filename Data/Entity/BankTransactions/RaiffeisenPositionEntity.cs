using System;

namespace BankDataDownloader.Data.Entity.BankTransactions
{
    public class RaiffeisenPositionEntity : PositionEntity,
        IEntityEqualityComparer<RaiffeisenPositionEntity>
    {
        public Func<RaiffeisenPositionEntity, bool> Func(RaiffeisenPositionEntity otherEntity)
        {
            return
                entity =>
                    entity.DateTime == otherEntity.DateTime && entity.Isin == otherEntity.Isin &&
                    entity.Amount == otherEntity.Amount;
        }
    }
}