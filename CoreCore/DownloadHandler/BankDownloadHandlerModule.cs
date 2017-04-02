using System;
using System.Reflection;
using Autofac;
using Autofac.Features.AttributeFilters;
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
