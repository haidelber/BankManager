using System;
using System.Globalization;

namespace BankDataDownloader.Core.Parser.Impl
{
    public class DateTimeValueParser : IValueParser
    {
        public object Parse(string toParse)
        {
            return DateTime.Parse(toParse, CultureInfo.InvariantCulture,
                DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeLocal);
        }
    }
}