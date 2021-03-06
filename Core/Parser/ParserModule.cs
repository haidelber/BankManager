﻿using Autofac;
using BankManager.Common;
using BankManager.Core.Parser.Impl;

namespace BankManager.Core.Parser
{
    public class ParserModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<StringValueParser>().Keyed<IValueParser>(Constants.UniqueContainerKeys.ValueParserString);
            builder.RegisterType<DateTimeValueParser>().Keyed<IValueParser>(Constants.UniqueContainerKeys.ValueParserDateTime);
            builder.RegisterType<DateTimeExactValueParser>().Keyed<IValueParser>(Constants.UniqueContainerKeys.ValueParserDateTimeExact);
            builder.RegisterType<ExcelDateTimeParser>().Keyed<IValueParser>(Constants.UniqueContainerKeys.ValueParserExcelDateTime);
            builder.RegisterType<EnglishDecimalValueParser>().Keyed<IValueParser>(Constants.UniqueContainerKeys.ValueParserEnglishDecimal);
            builder.RegisterType<GermanDecimalValueParser>().Keyed<IValueParser>(Constants.UniqueContainerKeys.ValueParserGermanDecimal);
            builder.RegisterType<EnumValueParser>().Keyed<IValueParser>(Constants.UniqueContainerKeys.ValueParserEnum);
            builder.RegisterType<ChainedValueParser>().Keyed<IValueParser>(Constants.UniqueContainerKeys.ValueParserChained);
            builder.RegisterType<SplitValueParser>().Keyed<IValueParser>(Constants.UniqueContainerKeys.ValueParserSplit);
        }
    }
}
