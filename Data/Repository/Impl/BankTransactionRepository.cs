using System.Linq;
using BankManager.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace BankManager.Data.Repository.Impl
{
    public class BankTransactionRepository<TEntity> : Repository<TEntity>, IBankTransactionRepository<TEntity> where TEntity : TransactionEntity
    {
        public BankTransactionRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public IQueryable<TEntity> GetAllForAccountId(long id)
        {
            return Query().Include(entity => entity.Account).Where(entity => entity.Account.Id == id);
        }

        public decimal TransactionSumForAccountId(long id)
        {
            return GetAllForAccountId(id).ToList().Sum(entity => entity.Amount);
        }
    }
}