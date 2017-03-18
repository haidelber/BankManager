using System;

namespace BankDataDownloader.Data.Entity.BankTransactions
{
    public class FlatexTransactionEntity : BankTransactionEntity, IEntityEqualityComparer<FlatexTransactionEntity>
    {
        public string Bic { get; set; }
        public string Iban { get; set; }
        public string TransactionNumber { get; set; }

        public Func<FlatexTransactionEntity, bool> Func(FlatexTransactionEntity otherEntity)
        {
            return entity =>
                     entity.AvailabilityDate == otherEntity.AvailabilityDate && /*entity.Text == otherEntity.Text &&*/
                     entity.Amount == otherEntity.Amount && entity.PostingDate == otherEntity.PostingDate &&
                     entity.CurrencyIso == otherEntity.CurrencyIso && entity.TransactionNumber == otherEntity.TransactionNumber;
        }
    }
}