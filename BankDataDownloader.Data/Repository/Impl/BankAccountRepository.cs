using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankDataDownloader.Common.Extensions;
using BankDataDownloader.Data.Entity;

namespace BankDataDownloader.Data.Repository.Impl
{
    public class BankAccountRepository : Repository<BankAccountEntity>, IBankAccountRepository
    {
        public BankAccountRepository(DbContext context) : base(context)
        {
        }

        public BankAccountEntity GetByIban(string iban)
        {
            var cleanIban = iban.CleanString();
            return Query().SingleOrDefault(entity => entity.Iban == cleanIban);
        }

        public BankAccountEntity GetByAccountNumberAndBankName(string accountNumber, string bankName)
        {
            var cleanAccountNumber = accountNumber.CleanString();
            return Query().SingleOrDefault(entity => entity.AccountNumber == cleanAccountNumber && entity.BankName == bankName);
        }
    }
}
