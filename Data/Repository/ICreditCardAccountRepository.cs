using BankManager.Data.Entity;

namespace BankManager.Data.Repository
{
    public interface ICreditCardAccountRepository : IRepository<CreditCardEntity>
    {
        CreditCardEntity GetByCreditCardNumber(string creditCardNumber);
        CreditCardEntity GetByAccountNumberAndBankName(string accountNumber, string bankName);
    }
}