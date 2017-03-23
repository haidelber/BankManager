using System;

namespace BankManager.Ui.Model.Account
{
    public class PortfolioModel
    {
        public Guid Id { get; set; }
        public string BankName { get; set; }
        public string AccountName { get; set; }
        public string PortfolioNumber { get; set; }

    }
}