using System.Reflection;
using Autofac;
using BankManager.Common.Converter;
using BankManager.Core.Helper.Automapper;
using BankManager.Core.Parser;
using BankManager.Core.ValueProvider.Impl;
using NLog;
using Module = Autofac.Module;

namespace BankManager.Core.Configuration
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
