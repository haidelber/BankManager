using System;

namespace BankManager.Ui.Model.Transaction
{
    public class PortfolioPositionModel
    {
        public Guid Id { get; set; }
        public string Isin { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateTime { get; set; }

        public decimal CurrentValue { get; set; }
        public string CurrentValueCurrencyIso { get; set; }

        public decimal OriginalValue { get; set; }
        public string OriginalValueCurrencyIso { get; set; }

        public Guid PortfolioId { get; set; }
    }
}
