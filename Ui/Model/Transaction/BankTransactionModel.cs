using System;

namespace BankManager.Ui.Model.Transaction
{
    public class BankTransactionModel
    {
        public Guid Id { get; set; }
        /// <summary>
        /// Valuta
        /// </summary>
        public DateTime AvailabilityDate { get; set; }
        public DateTime PostingDate { get; set; }
        public string Text { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyIso { get; set; }

        public Guid AccountId { get; set; }
    }
}
