using System;

namespace BankDataDownloader.Data.Entity.BankTransactions
{
    public class RaiffeisenTransactionEntity : BankTransactionEntity, IEntityEqualityComparer<RaiffeisenTransactionEntity>
    {
        public Func<RaiffeisenTransactionEntity, bool> Func(RaiffeisenTransactionEntity otherEntity)
        {
            return
                entity =>
                    entity.AvailabilityDate == otherEntity.AvailabilityDate && /*entity.Text == otherEntity.Text &&*/
                    entity.Amount == otherEntity.Amount && entity.PostingDate == otherEntity.PostingDate &&
                    entity.CurrencyIso == otherEntity.CurrencyIso;
        }
    }
}