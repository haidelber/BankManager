using Autofac;
using Autofac.Core;
using BankDataDownloader.Common;
using BankDataDownloader.Common.Model.Configuration;
using BankDataDownloader.Core.DownloadHandler.Impl;
using BankDataDownloader.Core.Parser.Impl;
using BankDataDownloader.Data.Entity;
using CsvHelper;

namespace BankDataDownloader.Core.Parser
{
    public class ParserModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CsvParser<RaiffeisenTransactionEntity>>()
                .WithParameter(
                    new ResolvedParameter(
                        (param, ctx) => param.ParameterType == typeof(FileParserConfiguration),
                        (pi, context) =>
                            context.ResolveNamed<FileParserConfiguration>(
                                Constants.UniqueContainerKeys.FileParseRaiffeisen)));
        }
    }
}
