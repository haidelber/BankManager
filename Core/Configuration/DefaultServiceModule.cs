using System.Reflection;
using Autofac;
using BankDataDownloader.Common.Converter;
using BankDataDownloader.Core.DownloadHandler;
using BankDataDownloader.Core.Parser;
using BankDataDownloader.Core.ValueProvider.Impl;
using Module = Autofac.Module;

namespace BankDataDownloader.Core.Configuration
{
    public class DefaultServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            var core = Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(core).Where(t => t.Name.EndsWith("Service")).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(core).Where(t => t.Name.EndsWith("ValueProvider")).Except<KeePassPasswordValueProvider>().AsImplementedInterfaces();
            builder.RegisterType<KeePassPasswordValueProvider>().AsImplementedInterfaces().SingleInstance();

            //Configuration Service + configuration model registration
            builder.RegisterModule<DefaultJsonModule>();
            builder.RegisterModule<ConfigurationModule>();
            builder.RegisterModule<BankDownloadHandlerModule>();
            builder.RegisterModule<ParserModule>();
        }
    }
}
