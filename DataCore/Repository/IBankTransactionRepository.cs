using System.Linq;
using BankDataDownloader.Data.Entity;

namespace BankDataDownloader.Data.Repository
{
    public interface IBankTransactionRepository : IRepository<TransactionEntity>
    {
        IQueryable<TransactionEntity> GetAllForAccountId(long id);
        decimal TransactionSumForAccountId(long id);
    }
}
