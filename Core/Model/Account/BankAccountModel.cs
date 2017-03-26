namespace BankDataDownloader.Core.Model.Account
{
    public class BankAccountModel : AccountModel
    {
        public string Iban { get; set; }
        public string AccountNumber { get; set; }
    }
}
