using System.Linq;
using BankManager.Common.Extensions;
using BankManager.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace BankManager.Data.Repository.Impl
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