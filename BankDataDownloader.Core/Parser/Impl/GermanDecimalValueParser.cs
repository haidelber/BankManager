using System.Globalization;

namespace BankDataDownloader.Core.Parser.Impl
{
    public class GermanDecimalValueParser : IValueParser
    {
        public object Parse(object toParse)
        {
            return decimal.Parse(toParse.ToString(), new CultureInfo("de"));
        }
    }
}