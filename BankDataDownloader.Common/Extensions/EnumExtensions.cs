using System;
using BankDataDownloader.Common.Model.Configuration;

namespace BankDataDownloader.Common.Extensions
{
    public static class EnumExtensions
    {
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
                default:
                    throw new ArgumentOutOfRangeException(nameof(parserEnum), parserEnum, null);
            }
        }
    }
}
