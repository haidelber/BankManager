using System;

namespace BankManager.Data.Entity.BankTransactions
{
    public class DkbCreditTransactionEntity : TransactionForeignCurrencyEntity, IEntityEqualityComparer<DkbCreditTransactionEntity>
    {
        public Func<DkbCreditTransactionEntity, bool> Func(DkbCreditTransactionEntity otherEntity)
        {
            return
                entity =>
                    entity.AvailabilityDate == otherEntity.AvailabilityDate && entity.UniqueId == otherEntity.UniqueId &&
                    entity.Amount == otherEntity.Amount && entity.PostingDate == otherEntity.PostingDate &&
                    entity.CurrencyIso == otherEntity.CurrencyIso;
        }
    }
}