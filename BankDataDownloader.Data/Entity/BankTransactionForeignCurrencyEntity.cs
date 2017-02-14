using System;

namespace BankDataDownloader.Data.Entity
{
    public abstract class BankTransactionForeignCurrencyEntity : BankTransactionEntity
    {
        public decimal AmountForeignCurrency { get; set; }
        public string ForeignCurrencyIso { get; set; }
        public decimal ExchangeRate { get; set; }
    }
}