using System.Text.RegularExpressions;

namespace BankManager.Core.Parser.Impl
{
    public class StringValueParser : IValueParser
    {
        public object Parse(object toParse)
        {
            var str = toParse.ToString();
            str = str.Replace('\0', ' ');
            str = Regex.Replace(str, @"\s+", " ").Trim();
            return string.IsNullOrEmpty(str) ? null : str;
        }
    }
}
