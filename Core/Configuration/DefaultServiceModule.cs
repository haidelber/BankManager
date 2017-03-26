using System.Reflection;
using Autofac;
using Autofac.Extras.AttributeMetadata;
using BankDataDownloader.Common.Converter;
using BankDataDownloader.Core.DownloadHandler;
using BankDataDownloader.Core.Helper.Automapper;
using BankDataDownloader.Core.Parser;
using BankDataDownloader.Core.ValueProvider.Impl;
using NLog;
using Module = Autofac.Module;

namespace BankDataDownloader.Core.Configuration
{
    public class DefaultServiceModule : Module
    {
        public readonly Logger Logger = LogManager.GetCurrentClassLogger();

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            Logger.Info($"Registering {GetType().Name}..");

            var core = Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(core).Where(t => t.Name.EndsWith("Service")).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(core).Where(t => t.Name.EndsWith("ValueProvider")).Except<KeePassPasswordValueProvider>().AsImplementedInterfaces();
            builder.RegisterType<KeePassPasswordValueProvider>().AsImplementedInterfaces().SingleInstance();

            //Configuration Service + configuration model registration
            builder.RegisterModule<DefaultJsonModule>();
            builder.RegisterModule<ConfigurationModule>();
            //builder.RegisterModule<AttributedMetadataModule>();
            //builder.RegisterModule<BankDownloadHandlerModule>();
            builder.RegisterModule<ParserModule>();
            builder.RegisterModule<AutoMapperModule>();
        }
    }
}
