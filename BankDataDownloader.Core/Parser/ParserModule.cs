using System;
using Autofac;
using Autofac.Core;
using BankDataDownloader.Common;
using BankDataDownloader.Common.Model.Configuration;
using BankDataDownloader.Core.DownloadHandler.Impl;
using BankDataDownloader.Core.Parser.Impl;
using CsvParser = BankDataDownloader.Core.Parser.Impl.CsvParser;

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


            builder.RegisterType<CsvParser>()
                .WithParameter(
                    new ResolvedParameter(
                        (param, ctx) => param.ParameterType == typeof (FileParserConfiguration),
                        (pi, context) =>
                            context.ResolveNamed<FileParserConfiguration>(
                                Constants.UniqueContainerKeys.FileParserRaiffeisen)))
                .Named<IFileParser>(Constants.UniqueContainerKeys.FileParserRaiffeisen);
        }
    }
}
