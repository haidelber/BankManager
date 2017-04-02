namespace BankManager.Core.Parser.Impl
{
    public class StringValueParser : IValueParser
    {
        public object Parse(object toParse)
        {
            var str = toParse.ToString().Trim();
            return string.IsNullOrEmpty(str) ? null : str;
        }
    }
}
