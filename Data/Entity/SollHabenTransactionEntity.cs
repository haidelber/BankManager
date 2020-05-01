using System;

namespace BankManager.Data.Entity
{
    public class SollHabenTransactionEntity : TransactionEntity
    {
        public decimal Soll { get; set; }
        public decimal Haben { get; set; }

        public override decimal Amount
        {
            get => Haben - Soll;
            set
            {
                if (value >= 0)
                {
                    Haben = Math.Abs(value);
                    Soll = 0;
                }
                else
                {
                    Haben = 0;
                    Soll = Math.Abs(value);
                }
            }
        }

        public override string ToString()
        {
            return $"{AvailabilityDate} {CurrencyIso} -{Soll} +{Haben} {Text}";
        }
    }
}