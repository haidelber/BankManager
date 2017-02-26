using BankDataDownloader.Data.Entity;

namespace BankDataDownloader.Data.Repository
{
    public interface IAccountRepository : IRepository<AccountEntity>
    {
        AccountEntity GetByAccountNameAndBankName(string accountName, string bankName);
    }
}