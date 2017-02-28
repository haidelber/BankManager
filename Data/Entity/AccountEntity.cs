using System;
using System.Collections.Generic;

namespace BankDataDownloader.Data.Entity
{
    public class AccountEntity : EntityBase, IEntityEqualityComparer<AccountEntity>
    {
        public string BankName { get; set; }
        public string AccountName { get; set; }

        public virtual ICollection<BankTransactionEntity> Transactions { get; set; }

        public AccountEntity()
        {
            Transactions = new List<BankTransactionEntity>();
        }

        public Func<AccountEntity, bool> Func(AccountEntity otherEntity)
        {
            return
                entity => entity.BankName == otherEntity.BankName && entity.AccountName == otherEntity.AccountName;
        }

        public override string ToString()
        {
            return $"{BankName} {AccountName}";
        }
    }
}