using System.Collections.Generic;
using System.Linq;
using Autofac;
using BankDataDownloader.Common.Model.Configuration;
using BankDataDownloader.Core.Extension;

namespace BankDataDownloader.Core.Parser.Impl
{
    public class ChainedValueParser : IValueParser
    {
        public IEnumerable<ValueParser> ParserChain { get; }
        public IEnumerable<Dictionary<string, object>> ValueParserParameter { get; }
        public IComponentContext ComponentContext { get; set; }

        public ChainedValueParser(IEnumerable<ValueParser> parserChain, IEnumerable<Dictionary<string, object>> valueParserParameter, IComponentContext componentContext)
        {
            ParserChain = parserChain;
            ValueParserParameter = valueParserParameter;
            ComponentContext = componentContext;
        }

        public object Parse(object toParse)
        {
            var chain = ParserChain.ToList();
            var param = ValueParserParameter.ToList();
            var currentValue = toParse;
            for (var i = 0; i < chain.Count; i++)
            {
                if (currentValue == null)
                {
                    return null;
                }
                currentValue = chain[i].ResolveParser(ComponentContext, param[i]).Parse(currentValue);
            }
            return currentValue;
        }
    }
}