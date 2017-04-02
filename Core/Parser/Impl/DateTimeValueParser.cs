using System;
using System.Globalization;

namespace BankDataDownloader.Core.Parser.Impl
{
    public class DateTimeValueParser : IValueParser
    {
        public object Parse(object toParse)
        {
            return DateTime.Parse(toParse.ToString(), CultureInfo.InvariantCulture,
                DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeLocal);
        }
    }
}