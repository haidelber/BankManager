using System.Data.Entity;
using System.Linq;
using BankDataDownloader.Data.Entity;

namespace BankDataDownloader.Data.Repository.Impl
{
    public class BankTransactionRepository : Repository<BankTransactionEntity>, IBankTransactionRepository
    {
        public BankTransactionRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public IQueryable<BankTransactionEntity> GetAllForAccountId(long id)
        {
            return Query().Include(entity => entity.Account).Where(entity => entity.Account.Id == id);
        }

        public decimal TransactionSumForAccountId(long id)
        {
            return GetAllForAccountId(id).ToList().Sum(entity => entity.Amount);
        }
    }
}