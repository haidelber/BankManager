using System.Collections.Generic;
using System.Linq;
using Autofac;
using BankManager.Common.Model.Configuration;
using BankManager.Core.Parser;

namespace BankManager.Core.Extension
{
    public static class ParserExtensions
    {
        public static IValueParser ResolveParser(this PropertySourceConfiguration conf, IComponentContext context)
        {
            var parameters =
                conf.ValueParserParameter.Select(param => new NamedParameter(param.Key, param.Value));
            return context.ResolveKeyed<IValueParser>(conf.Parser.GetContainerName(), parameters);
        }

        public static IValueParser ResolveParser(this ValueParser parser, IComponentContext context, IDictionary<string, object> parameter)
        {
            var parameters =
                parameter.Select(param => new NamedParameter(param.Key, param.Value));
            return context.ResolveKeyed<IValueParser>(parser.GetContainerName(), parameters);
        }
    }
}
