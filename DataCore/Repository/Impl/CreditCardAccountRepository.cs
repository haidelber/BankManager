using System.Linq;
using BankDataDownloader.Common.Extensions;
using BankDataDownloader.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace BankDataDownloader.Data.Repository.Impl
{
    public class CreditCardAccountRepository : Repository<CreditCardEntity>, ICreditCardAccountRepository
    {
        public CreditCardAccountRepository(DbContext context) : base(context)
        {
        }

        public CreditCardEntity GetByCreditCardNumber(string creditCardNumber)
        {
            var cleanCreditCardNumber = creditCardNumber.CleanString();
            return Query().SingleOrDefault(entity => entity.CreditCardNumber == cleanCreditCardNumber);
        }

        public CreditCardEntity GetByAccountNumberAndBankName(string accountNumber, string bankName)
        {
            var cleanAccountNumber = accountNumber.CleanString();
            return Query().SingleOrDefault(entity => entity.AccountNumber == cleanAccountNumber && entity.BankName == bankName);
        }
    }
}