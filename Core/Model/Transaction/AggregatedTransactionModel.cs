namespace BankManager.Core.Model.Transaction
{
    public class AggregatedTransactionModel
    {
        public int Year { get; set; }
        public int Month { get; set; }

        public decimal Average { get; set; }
        public decimal StdDev { get; set; }
    }
}