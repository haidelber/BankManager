using System;

namespace BankManager.Data.Entity.BankTransactions
{
    public class DkbTransactionEntity : TransactionEntity, IEntityEqualityComparer<DkbTransactionEntity>
    {
        public string PostingText { get; set; }
        public string SenderReceiver { get; set; }
        public string OtherIban { get; set; }
        public string OtherBic { get; set; }
        public string CreditorId { get; set; }
        public string MandateReference { get; set; }
        public string CustomerReference { get; set; }

        public Func<DkbTransactionEntity, bool> Func(DkbTransactionEntity otherEntity)
        {
            return
                entity =>
                    entity.AvailabilityDate == otherEntity.AvailabilityDate && /*entity.Text == otherEntity.Text &&*/
                    entity.Amount == otherEntity.Amount && entity.PostingDate == otherEntity.PostingDate &&
                    entity.CurrencyIso == otherEntity.CurrencyIso;
        }
    }
}