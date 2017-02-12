namespace BankDataDownloader.Data.Entity
{
    public abstract class BankTransactionForeignCurrency : BankTransaction
    {
        public decimal AmountForeignCurrency { get; set; }
        public string ForeignCurrencyIso { get; set; }
        public decimal ExchangeRate { get; set; }
    }
}