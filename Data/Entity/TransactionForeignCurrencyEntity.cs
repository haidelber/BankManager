namespace BankManager.Data.Entity
{
    public class TransactionForeignCurrencyEntity : TransactionEntity
    {
        public decimal AmountForeignCurrency { get; set; }
        public string ForeignCurrencyIso { get; set; }
        public decimal ExchangeRate { get; set; }
    }
}