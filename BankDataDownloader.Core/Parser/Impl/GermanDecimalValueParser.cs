using System.Globalization;

namespace BankDataDownloader.Core.Parser.Impl
{
    public class GermanDecimalValueParser : IValueParser
    {
        public object Parse(string toParse)
        {
            return decimal.Parse(toParse, new CultureInfo("de"));
        }
    }
}