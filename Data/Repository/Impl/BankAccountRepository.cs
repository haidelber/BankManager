﻿using System.Linq;
using BankManager.Common.Extensions;
using BankManager.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace BankManager.Data.Repository.Impl
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

        public BankAccountEntity GetByAccountNameAndBankName(string accountName, string bankName)
        {
            return
                Query()
                    .SingleOrDefault(
                        entity => entity.AccountName == accountName && entity.BankName == bankName);
        }
    }
}
