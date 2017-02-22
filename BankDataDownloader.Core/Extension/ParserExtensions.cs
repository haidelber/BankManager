using System.Linq;
using Autofac;
using BankDataDownloader.Common.Extensions;
using BankDataDownloader.Common.Model.Configuration;
using BankDataDownloader.Core.Parser;

namespace BankDataDownloader.Core.Extension
{
    public static class ParserExtensions
    {
        public static IValueParser ResolveParser(this PropertySourceConfiguration conf, IComponentContext context)
        {
            var parameters =
                conf.ValueParserParameter.Select(param => new NamedParameter(param.Key, param.Value));
            return context.ResolveNamed<IValueParser>(conf.Parser.GetContainerName(), parameters);
        }
    }
}
