using System;
using System.ComponentModel.DataAnnotations;

namespace BankManager.Data.Entity
{
    public class TransactionEntity : EntityBase
    {
        /// <summary>
        /// Valuta
        /// </summary>
        [Required]
        public DateTime AvailabilityDate { get; set; }
        public DateTime PostingDate { get; set; }
        public int? UniqueId { get; set; }
        public virtual string Text { get; set; }
        [Required]
        public decimal Amount { get; set; }
        public string CurrencyIso { get; set; }

        public virtual AccountEntity Account { get; set; }

        public override string ToString()
        {
            return $"{AvailabilityDate} {CurrencyIso} {Amount} {Text}";
        }
    }
}
