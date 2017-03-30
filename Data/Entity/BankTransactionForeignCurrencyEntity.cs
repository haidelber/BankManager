namespace BankDataDownloader.Data.Entity
{
    public class BankTransactionForeignCurrencyEntity : BankTransactionEntity
    {
        public decimal AmountForeignCurrency { get; set; }
        public string ForeignCurrencyIso { get; set; }
        public decimal ExchangeRate { get; set; }
    }
}