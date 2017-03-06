using System;
using System.Reflection;
using Autofac;
using Autofac.Extras.AttributeMetadata;
using Autofac.Features.AttributeFilters;
using BankDataDownloader.Common;
using BankDataDownloader.Common.Model.Configuration;
using BankDataDownloader.Core.DownloadHandler.Impl;
using BankDataDownloader.Data;
using Module = Autofac.Module;

namespace BankDataDownloader.Core.DownloadHandler
{
    [Obsolete]
    public class BankDownloadHandlerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var core = Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(core).AssignableTo<IBankDownloadHandler>().AsSelf().AsImplementedInterfaces().WithAttributeFiltering();
        }
    }
}
