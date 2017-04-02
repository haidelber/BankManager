using System;

namespace BankManager.Core.Parser.Impl
{
    public class ExcelDateTimeParser : IValueParser
    {
        public object Parse(object toParse)
        {
            var daysFrom1900 = double.Parse(toParse.ToString());
            return DateTime.FromOADate(daysFrom1900);
        }
    }
}