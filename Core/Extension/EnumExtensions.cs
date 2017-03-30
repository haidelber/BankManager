using System;
using BankDataDownloader.Common.Model.Configuration;
using BankDataDownloader.Core.Model.Account;
using BankDataDownloader.Core.Model.Import;
using BankDataDownloader.Data.Entity;

namespace BankDataDownloader.Common.Extensions
{
    public static class EnumExtensions
    {
        public static Type GetTransactionType(this AccountType accountType)
        {
            switch (accountType)
            {
                case AccountType.BankAccount:
                    return typeof(BankTransactionEntity);
                case AccountType.CreditCardAccount:
                    return typeof(BankTransactionForeignCurrencyEntity);
                case AccountType.Portfolio:
                    return typeof(PortfolioPositionEntity);
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
                    return typeof(CreditCardAccountEntity);
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
