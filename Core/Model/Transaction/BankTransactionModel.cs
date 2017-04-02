using System;

namespace BankDataDownloader.Core.Model.Transaction
{
    public class BankTransactionModel
    {
        public long Id { get; set; }
        /// <summary>
        /// Valuta
        /// </summary>
        public DateTime AvailabilityDate { get; set; }
        public DateTime PostingDate { get; set; }
        public string Text { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyIso { get; set; }

        public long AccountId { get; set; }
    }
}
