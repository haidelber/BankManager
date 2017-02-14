using System;

namespace BankDataDownloader.Data.Entity.BankTransactions
{
    public class Number26TransactionEntity : BankTransactionForeignCurrencyEntity, IEntityEqualityComparer<Number26TransactionEntity>
    {
        public string Payee { get; set; }
        public string PayeeAccountNumber { get; set; }
        public string TransactionType { get; set; }
        public string PaymentReference { get; set; }
        public string Category { get; set; }

        public Func<Number26TransactionEntity, bool> Func(Number26TransactionEntity otherEntity)
        {
            return
                entity =>
                    entity.AvailabilityDate == otherEntity.AvailabilityDate &&
                    entity.PostingDate == otherEntity.PostingDate && entity.Text == otherEntity.Text &&
                    entity.Amount == otherEntity.Amount && entity.CurrencyIso == otherEntity.CurrencyIso &&
                    entity.AmountForeignCurrency == otherEntity.AmountForeignCurrency &&
                    entity.ForeignCurrencyIso == otherEntity.ForeignCurrencyIso &&
                    entity.ExchangeRate == otherEntity.ExchangeRate;
        }
    }
}