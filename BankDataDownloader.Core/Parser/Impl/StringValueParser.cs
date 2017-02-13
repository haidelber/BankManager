using System;

namespace BankDataDownloader.Core.Parser.Impl
{
    public class StringValueParser : IValueParser
    {
        public object Parse(string toParse)
        {
            return toParse.Trim();
        }
    }
}
