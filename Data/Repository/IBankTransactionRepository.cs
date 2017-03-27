using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankDataDownloader.Data.Entity;

namespace BankDataDownloader.Data.Repository
{
    public interface IBankTransactionRepository : IRepository<BankTransactionEntity>
    {
        IQueryable<BankTransactionEntity> GetAllForAccountId(long id);
        decimal TransactionSumForAccountId(long id);
    }
}
