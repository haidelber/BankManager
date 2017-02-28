using Autofac;
using Autofac.Core;
using BankDataDownloader.Common;
using BankDataDownloader.Common.Model.Configuration;
using BankDataDownloader.Core.DownloadHandler.Impl;

namespace BankDataDownloader.Core.DownloadHandler
{
    public class BankDownloadHandlerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DkbDownloadHandler>()
                .WithParameter(
                    new ResolvedParameter(
                        (param, ctx) => param.ParameterType == typeof(DownloadHandlerConfiguration),
                        (pi, context) =>
                            context.ResolveNamed<DownloadHandlerConfiguration>(
                                Constants.UniqueContainerKeys.DownloadHandlerDkb)));
            builder.RegisterType<Number26DownloadHandler>()
                .WithParameter(
                    new ResolvedParameter(
                        (param, ctx) => param.ParameterType == typeof(DownloadHandlerConfiguration),
                        (pi, context) =>
                            context.ResolveNamed<DownloadHandlerConfiguration>(
                                Constants.UniqueContainerKeys.DownloadHandlerNumber26)));
            builder.RegisterType<RaiffeisenDownloadHandler>()
                .WithParameter(
                    new ResolvedParameter(
                        (param, ctx) => param.ParameterType == typeof(DownloadHandlerConfiguration),
                        (pi, context) =>
                            context.ResolveNamed<DownloadHandlerConfiguration>(
                                Constants.UniqueContainerKeys.DownloadHandlerRaiffeisen)));
            builder.RegisterType<RciDownloadHandler>()
                .WithParameter(
                    new ResolvedParameter(
                        (param, ctx) => param.ParameterType == typeof(DownloadHandlerConfiguration),
                        (pi, context) =>
                            context.ResolveNamed<DownloadHandlerConfiguration>(
                                Constants.UniqueContainerKeys.DownloadHandlerRci)));
            builder.RegisterType<SantanderDownloadHandler>()
                .WithParameter(
                    new ResolvedParameter(
                        (param, ctx) => param.ParameterType == typeof(DownloadHandlerConfiguration),
                        (pi, context) =>
                            context.ResolveNamed<DownloadHandlerConfiguration>(
                                Constants.UniqueContainerKeys.DownloadHandlerSantander)));
            builder.RegisterType<PayPalDownloadHandler>()
                .WithParameter(
                    new ResolvedParameter(
                        (param, ctx) => param.ParameterType == typeof(DownloadHandlerConfiguration),
                        (pi, context) =>
                            context.ResolveNamed<DownloadHandlerConfiguration>(
                                Constants.UniqueContainerKeys.DownloadHandlerPayPal)));
        }
    }
}
