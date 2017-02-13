using System.Globalization;

namespace BankDataDownloader.Core.Parser.Impl
{
    public class EnglishDecimalValueParser : IValueParser
    {
        public object Parse(string toParse)
        {
            return decimal.Parse(toParse, new CultureInfo("en"));
        }
    }
}