namespace BankManager.Core.Model.Account
{
    public class AccountModel
    {
        public long Id { get; set; }
        public string BankName { get; set; }
        public string AccountName { get; set; }

        public bool Active { get; set; }
    }
}