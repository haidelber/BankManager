using System.Data.Entity;
using System.Linq;
using BankDataDownloader.Data.Entity;

namespace BankDataDownloader.Data.Repository.Impl
{
    public class AccountRepository : Repository<AccountEntity>, IAccountRepository
    {
        public AccountRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public AccountEntity GetByAccountNameAndBankName(string accountName, string bankName)
        {
            return Query().SingleOrDefault(entity => entity.AccountName == accountName && entity.BankName == bankName);
        }
    }
}