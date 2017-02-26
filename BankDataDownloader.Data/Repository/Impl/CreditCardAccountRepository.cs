using System.Data.Entity;
using System.Linq;
using BankDataDownloader.Common.Extensions;
using BankDataDownloader.Data.Entity;

namespace BankDataDownloader.Data.Repository.Impl
{
    public class CreditCardAccountRepository : Repository<CreditCardAccountEntity>, ICreditCardAccountRepository
    {
        public CreditCardAccountRepository(DbContext context) : base(context)
        {
        }

        public CreditCardAccountEntity GetByCreditCardNumber(string creditCardNumber)
        {
            var cleanCreditCardNumber = creditCardNumber.CleanString();
            return Query().SingleOrDefault(entity => entity.CreditCardNumber == cleanCreditCardNumber);
        }

        public CreditCardAccountEntity GetByAccountNumberAndBankName(string accountNumber, string bankName)
        {
            var cleanAccountNumber = accountNumber.CleanString();
            return Query().SingleOrDefault(entity => entity.AccountNumber == cleanAccountNumber && entity.BankName == bankName);
        }
    }
}