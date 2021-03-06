﻿using System;
using BankManager.Common;
using BankManager.Common.Model.Configuration;
using BankManager.Core.Model.Account;
using BankManager.Data.Entity;

namespace BankManager.Core.Extension
{
    public static class EnumExtensions
    {
        public static Type GetTransactionType(this AccountType accountType)
        {
            switch (accountType)
            {
                case AccountType.BankAccount:
                    return typeof(TransactionEntity);
                case AccountType.CreditCardAccount:
                    return typeof(TransactionForeignCurrencyEntity);
                case AccountType.Portfolio:
                    return typeof(PositionEntity);
                default:
                    throw new ArgumentOutOfRangeException(nameof(accountType), accountType, null);
            }
        }
        public static Type GetAccountType(this AccountType accountType)
        {
            switch (accountType)
            {
                case AccountType.BankAccount:
                    return typeof(BankAccountEntity);
                case AccountType.CreditCardAccount:
                    return typeof(CreditCardEntity);
                case AccountType.Portfolio:
                    return typeof(PortfolioEntity);
                default:
                    throw new ArgumentOutOfRangeException(nameof(accountType), accountType, null);
            }
        }

        public static string GetContainerName(this ValueParser parserEnum)
        {
            switch (parserEnum)
            {
                case ValueParser.String:
                    return Constants.UniqueContainerKeys.ValueParserString;
                case ValueParser.GermanDecimal:
                    return Constants.UniqueContainerKeys.ValueParserGermanDecimal;
                case ValueParser.EnglishDecimal:
                    return Constants.UniqueContainerKeys.ValueParserEnglishDecimal;
                case ValueParser.DateTime:
                    return Constants.UniqueContainerKeys.ValueParserDateTime;
                case ValueParser.Enum:
                    return Constants.UniqueContainerKeys.ValueParserEnum;
                case ValueParser.DateTimeExact:
                    return Constants.UniqueContainerKeys.ValueParserDateTimeExact;
                case ValueParser.Chained:
                    return Constants.UniqueContainerKeys.ValueParserChained;
                case ValueParser.Split:
                    return Constants.UniqueContainerKeys.ValueParserSplit;
                case ValueParser.ExcelDateTime:
                    return Constants.UniqueContainerKeys.ValueParserExcelDateTime;
                default:
                    throw new ArgumentOutOfRangeException(nameof(parserEnum), parserEnum, null);
            }
        }
    }
}
