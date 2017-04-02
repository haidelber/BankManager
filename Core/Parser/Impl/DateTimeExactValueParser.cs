using System;
using System.Collections.Generic;
using System.Globalization;

namespace BankManager.Core.Parser.Impl
{
    public class DateTimeExactValueParser : IValueParser
    {
        public IEnumerable<string> Formats { get; }

        public DateTimeExactValueParser(IEnumerable<string> formats)
        {
            Formats = formats;
        }

        public object Parse(object toParse)
        {
            foreach (var format in Formats)
            {
                try
                {
                    return Parse(toParse.ToString(), format);
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