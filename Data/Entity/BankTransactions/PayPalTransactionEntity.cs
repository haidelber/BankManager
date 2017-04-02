using System;

namespace BankManager.Data.Entity.BankTransactions
{
    public class PayPalTransactionEntity : TransactionForeignCurrencyEntity, IEntityEqualityComparer<PayPalTransactionEntity>
    {
        public string Type { get; set; }
        public decimal NetAmount { get; set; }
        public string FromEmailAddress { get; set; }
        public string ToEmailAddress { get; set; }
        public string TransactionId { get; set; }
        public string ReferenceTxnId { get; set; }
        public string InvoiceNumber { get; set; }


        public Func<PayPalTransactionEntity, bool> Func(PayPalTransactionEntity otherEntity)
        {
            return
                entity =>
                    entity.AvailabilityDate == otherEntity.AvailabilityDate && /*entity.Text == otherEntity.Text &&*/
                    entity.Amount == otherEntity.Amount && entity.PostingDate == otherEntity.PostingDate &&
                    entity.CurrencyIso == otherEntity.CurrencyIso && entity.Type == otherEntity.Type &&
                    entity.NetAmount == otherEntity.NetAmount && entity.FromEmailAddress == otherEntity.FromEmailAddress &&
                    entity.ToEmailAddress == otherEntity.ToEmailAddress &&
                    entity.TransactionId == otherEntity.TransactionId &&
                    entity.ReferenceTxnId == otherEntity.ReferenceTxnId &&
                    entity.InvoiceNumber == otherEntity.InvoiceNumber;
        }
    }
}