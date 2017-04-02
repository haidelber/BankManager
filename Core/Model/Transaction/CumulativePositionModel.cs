namespace BankManager.Core.Model.Transaction
{
    public class CumulativePositionModel : PortfolioPositionModel
    {
        public decimal ChangeToPrevious { get; set; }
        public decimal Cumulative { get; set; }
    }
}