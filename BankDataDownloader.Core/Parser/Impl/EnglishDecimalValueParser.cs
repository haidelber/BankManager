using System.Globalization;

namespace BankDataDownloader.Core.Parser.Impl
{
    public class EnglishDecimalValueParser : IValueParser
    {
        public object Parse(object toParse)
        {
            return string.IsNullOrWhiteSpace(toParse.ToString()) ? 0M : decimal.Parse(toParse.ToString(), new CultureInfo("en"));
        }
    }
}