namespace BankManager.Core.Model.Transaction
{
    public class CumulativePositionModel : PortfolioPositionModel
    {
        public decimal AmountChange { get; set; }
        public decimal ValuePerItemChange { get; set; }
        public decimal Cumulative { get; set; }
    }
}