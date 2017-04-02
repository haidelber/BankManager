namespace BankDataDownloader.Core.Parser.Impl
{
    public class SplitValueParser : IValueParser
    {
        public string SplitChars { get; }
        public int Index { get; }
        protected object DefaultValue { get; }

        public SplitValueParser(string splitChars, int index, object defaultValue)
        {
            SplitChars = splitChars;
            Index = index;
            DefaultValue = defaultValue;
        }

        public SplitValueParser(string splitChars, int index)
        {
            SplitChars = splitChars;
            Index = index;
            DefaultValue = null;
        }

        public object Parse(object toParse)
        {
            if (string.IsNullOrEmpty(toParse.ToString()))
            {
                return DefaultValue;
            }
            var splits = toParse.ToString().Split(SplitChars.ToCharArray());
            return Index > splits.Length ? DefaultValue : splits[Index];
        }
    }
}