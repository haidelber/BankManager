using System;

namespace BankDataDownloader.Data.Entity.BankTransactions
{
    public class FlatexPositionEntity : PositionEntity,
        IEntityEqualityComparer<FlatexPositionEntity>
    {
        public string Category { get; set; }
        public string Depository { get; set; }
        public string StockExchange { get; set; }

        public Func<FlatexPositionEntity, bool> Func(FlatexPositionEntity otherEntity)
        {
            return
                entity =>
                    entity.DateTime == otherEntity.DateTime && entity.Isin == otherEntity.Isin &&
                    entity.Amount == otherEntity.Amount;
        }
    }
}