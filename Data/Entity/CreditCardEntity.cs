using System;

namespace BankDataDownloader.Data.Entity
{
    public class CreditCardEntity : AccountEntity, IEntityEqualityComparer<CreditCardEntity>
    {
        public string CreditCardNumber { get; set; }
        public string AccountNumber { get; set; }

        public Func<CreditCardEntity, bool> Func(CreditCardEntity otherEntity)
        {
            return entity => entity.CreditCardNumber == otherEntity.CreditCardNumber && entity.AccountNumber == otherEntity.AccountNumber;
        }

        public override string ToString()
        {
            return $"{CreditCardNumber} {BankName} {AccountName} {AccountNumber}";
        }
    }
}