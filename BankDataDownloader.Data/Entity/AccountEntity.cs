using System;
using System.Collections.Generic;

namespace BankDataDownloader.Data.Entity
{
    public abstract class AccountEntity : EntityBase
    {
        public string BankName { get; set; }
        public string AccountName { get; set; }

        public virtual ICollection<BankTransactionEntity> Transactions { get; set; }

        protected AccountEntity()
        {
            Transactions = new List<BankTransactionEntity>();
        }
    }
}