using Autofac;
using BankDataDownloader.Common;
using BankDataDownloader.Core.Parser.Impl;

namespace BankDataDownloader.Core.Parser
{
    public class ParserModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<StringValueParser>().Named<IValueParser>(Constants.UniqueContainerKeys.ValueParserString);
            builder.RegisterType<DateTimeValueParser>().Named<IValueParser>(Constants.UniqueContainerKeys.ValueParserDateTime);
            builder.RegisterType<DateTimeExactValueParser>().Named<IValueParser>(Constants.UniqueContainerKeys.ValueParserDateTimeExact);
            builder.RegisterType<EnglishDecimalValueParser>().Named<IValueParser>(Constants.UniqueContainerKeys.ValueParserEnglishDecimal);
            builder.RegisterType<GermanDecimalValueParser>().Named<IValueParser>(Constants.UniqueContainerKeys.ValueParserGermanDecimal);
            builder.RegisterType<EnumValueParser>().Named<IValueParser>(Constants.UniqueContainerKeys.ValueParserEnum);
        }
    }
}
