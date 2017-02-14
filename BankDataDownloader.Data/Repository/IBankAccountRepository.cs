using BankDataDownloader.Data.Entity;

namespace BankDataDownloader.Data.Repository
{
    public interface IBankAccountRepository : IRepository<BankAccountEntity>
    {
        BankAccountEntity GetByIban(string iban);
        BankAccountEntity GetByAccountNumberAndBankName(string accountNumber, string bankName);
    }
}