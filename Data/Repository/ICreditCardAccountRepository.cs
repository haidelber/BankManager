using BankDataDownloader.Data.Entity;

namespace BankDataDownloader.Data.Repository
{
    public interface ICreditCardAccountRepository : IRepository<CreditCardEntity>
    {
        CreditCardEntity GetByCreditCardNumber(string creditCardNumber);
        CreditCardEntity GetByAccountNumberAndBankName(string accountNumber, string bankName);
    }
}