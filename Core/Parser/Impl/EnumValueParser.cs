using System;

namespace BankManager.Core.Parser.Impl
{
    public class EnumValueParser : IValueParser
    {
        public Type TargetType { get; }

        public EnumValueParser(Type targetType)
        {
            TargetType = targetType;
        }

        public object Parse(object toParse)
        {
            if (!TargetType.IsEnum) throw new NotSupportedException();
            return Enum.Parse(TargetType, toParse.ToString(), true);
        }
    }
}