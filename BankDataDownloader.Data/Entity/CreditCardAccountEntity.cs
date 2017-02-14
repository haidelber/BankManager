using System;

namespace BankDataDownloader.Data.Entity
{
    public class CreditCardAccountEntity : AccountEntity, IEntityEqualityComparer<CreditCardAccountEntity>
    {
        public string CreditCardNumber { get; set; }

        public Func<CreditCardAccountEntity, bool> Func(CreditCardAccountEntity otherEntity)
        {
            return entity => entity.CreditCardNumber == otherEntity.CreditCardNumber;
        }
    }
}