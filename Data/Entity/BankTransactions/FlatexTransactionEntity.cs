using System;

namespace BankDataDownloader.Data.Entity.BankTransactions
{
    public class FlatexTransactionEntity : BankTransactionEntity,IEntityEqualityComparer<FlatexTransactionEntity>
    {
        public Func<FlatexTransactionEntity, bool> Func(FlatexTransactionEntity otherEntity)
        {
            throw new NotImplementedException();
        }
    }
}