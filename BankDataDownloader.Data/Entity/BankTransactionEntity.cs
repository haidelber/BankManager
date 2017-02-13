using System;
using System.ComponentModel.DataAnnotations;

namespace BankDataDownloader.Data.Entity
{
    public abstract class BankTransactionEntity : EntityBase
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

        public override string ToString()
        {
            return $"{AvailabilityDate} {CurrencyIso} {Amount} {Text}";
        }

        protected bool Equals(BankTransactionEntity other)
        {
            return AvailabilityDate.Equals(other.AvailabilityDate) && PostingDate.Equals(other.PostingDate) && string.Equals(Text, other.Text, StringComparison.OrdinalIgnoreCase) && Amount == other.Amount && string.Equals(CurrencyIso, other.CurrencyIso, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BankTransactionEntity)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = AvailabilityDate.GetHashCode();
                hashCode = (hashCode * 397) ^ PostingDate.GetHashCode();
                hashCode = (hashCode * 397) ^ (Text != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(Text) : 0);
                hashCode = (hashCode * 397) ^ Amount.GetHashCode();
                hashCode = (hashCode * 397) ^ (CurrencyIso != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(CurrencyIso) : 0);
                return hashCode;
            }
        }

        public static bool operator ==(BankTransactionEntity left, BankTransactionEntity right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(BankTransactionEntity left, BankTransactionEntity right)
        {
            return !Equals(left, right);
        }
    }
}
