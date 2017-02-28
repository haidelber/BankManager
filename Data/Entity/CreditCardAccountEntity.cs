using System;

namespace BankDataDownloader.Data.Entity
{
    public class CreditCardAccountEntity : AccountEntity, IEntityEqualityComparer<CreditCardAccountEntity>
    {
        public string CreditCardNumber { get; set; }
        public string AccountNumber { get; set; }

        public Func<CreditCardAccountEntity, bool> Func(CreditCardAccountEntity otherEntity)
        {
            return entity => entity.CreditCardNumber == otherEntity.CreditCardNumber && entity.AccountNumber == otherEntity.AccountNumber;
        }

        public override string ToString()
        {
            return $"{CreditCardNumber} {BankName} {AccountName} {AccountNumber}";
        }
    }
}