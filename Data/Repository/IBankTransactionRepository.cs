using System.Linq;
using BankManager.Data.Entity;

namespace BankManager.Data.Repository
{
    public interface IBankTransactionRepository : IRepository<TransactionEntity>
    {
        IQueryable<TransactionEntity> GetAllForAccountId(long id);
        decimal TransactionSumForAccountId(long id);
    }
}
