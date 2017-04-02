using System.Linq;
using BankManager.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace BankManager.Data.Repository.Impl
{
    public class BankTransactionRepository : Repository<TransactionEntity>, IBankTransactionRepository
    {
        public BankTransactionRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public IQueryable<TransactionEntity> GetAllForAccountId(long id)
        {
            return Query().Include(entity => entity.Account).Where(entity => entity.Account.Id == id);
        }

        public decimal TransactionSumForAccountId(long id)
        {
            return GetAllForAccountId(id).ToList().Sum(entity => entity.Amount);
        }
    }
}