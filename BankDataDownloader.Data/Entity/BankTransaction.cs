using System;
using System.ComponentModel.DataAnnotations;

namespace BankDataDownloader.Data.Entity
{
    public abstract class BankTransaction : EntityBase
    {
        /// <summary>
        /// Valuta
        /// </summary>
        [Required]
        public DateTime AvailabilityDate { get; set; }
        public DateTime PostingDate { get; set; }
        public string Text { get; set; }
        [Required]
        public decimal Amount { get; set; }
        public string CurrencyIso { get; set; }
    }
}
