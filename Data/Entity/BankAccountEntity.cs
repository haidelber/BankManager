using System;

namespace BankManager.Data.Entity
{
    public class BankAccountEntity : AccountEntity, IEntityEqualityComparer<BankAccountEntity>
    {
        public string Iban { get; set; }
        public string AccountNumber { get; set; }

        public Func<BankAccountEntity, bool> Func(BankAccountEntity otherEntity)
        {
            return
                entity => entity.Iban == otherEntity.Iban;
        }

        public override string ToString()
        {
            return $"{Iban} {BankName} {AccountName}";
        }
    }
}
