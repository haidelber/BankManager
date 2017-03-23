namespace BankManager.Ui.Model.Transaction
{
    public class BankTransactionForeignCurrencyModel : BankTransactionModel
    {
        public decimal AmountForeignCurrency { get; set; }
        public string ForeignCurrencyIso { get; set; }
        public decimal ExchangeRate { get; set; }
    }
}