using BankManager.Data.Entity;

namespace BankManager.Data.Repository
{
    public interface IBankAccountRepository : IRepository<BankAccountEntity>
    {
        BankAccountEntity GetByIban(string iban);
        BankAccountEntity GetByAccountNumberAndBankName(string accountNumber, string bankName);
        BankAccountEntity GetByAccountNameAndBankName(string accountName, string bankName);
    }
}