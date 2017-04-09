namespace BankManager.Core.Model.Transaction
{
    public class AggregatedTransactionModel
    {
        public int Year { get; set; }
        public int Month { get; set; }

        public decimal Average { get; set; }
        public decimal AverageFlattened { get; set; }
        public decimal StdDev { get; set; }
        public decimal Median { get; set; }
        public decimal MedianFlattened { get; set; }
    }
}