using System;
using System.Collections.Generic;
using System.Globalization;

namespace BankDataDownloader.Core.Parser.Impl
{
    public class DateTimeExactValueParser : IValueParser
    {
        public IEnumerable<string> Formats { get; set; }

        public DateTimeExactValueParser(IEnumerable<string> formats)
        {
            Formats = formats;
        }

        public object Parse(string toParse)
        {
            foreach (var format in Formats)
            {
                try
                {
                    return Parse(toParse, format);
                }
                catch (FormatException)
                {

                }
            }
            return null;
        }

        private DateTime Parse(string toParse, string format)
        {
            return DateTime.ParseExact(toParse, format, CultureInfo.InvariantCulture);
        }
    }
}