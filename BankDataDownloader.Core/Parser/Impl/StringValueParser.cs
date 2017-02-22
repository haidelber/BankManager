namespace BankDataDownloader.Core.Parser.Impl
{
    public class StringValueParser : IValueParser
    {
        public object Parse(object toParse)
        {
            return toParse.ToString().Trim();
        }
    }
}
