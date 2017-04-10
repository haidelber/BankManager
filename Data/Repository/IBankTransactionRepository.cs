using System.Linq;
using BankManager.Data.Entity;

namespace BankManager.Data.Repository
{
    public interface IBankTransactionRepository<TEntity> : IRepository<TEntity> where TEntity:TransactionEntity
    {
        IQueryable<TEntity> GetAllForAccountId(long id);
        decimal TransactionSumForAccountId(long id);
    }
}
