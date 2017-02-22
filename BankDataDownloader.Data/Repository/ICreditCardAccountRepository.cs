using BankDataDownloader.Data.Entity;

namespace BankDataDownloader.Data.Repository
{
    public interface ICreditCardAccountRepository : IRepository<CreditCardAccountEntity>
    {
        CreditCardAccountEntity GetByCreditCardNumber(string creditCardNumber);
        CreditCardAccountEntity GetByAccountNumberAndBankName(string accountNumber, string bankName);
    }
}