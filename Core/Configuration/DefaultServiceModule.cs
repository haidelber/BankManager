using System.Reflection;
using Autofac;
using BankManager.Common.Converter;
using BankManager.Core.Helper.Automapper;
using BankManager.Core.Parser;
using BankManager.Core.Provider.Impl;
using Module = Autofac.Module;

namespace BankManager.Core.Configuration
{
    public class DefaultServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            var core = Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(core).Where(t => t.Name.EndsWith("Service")).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(core).Where(t => t.Name.EndsWith("Provider")).Except<KeePassPasswordProvider>().AsImplementedInterfaces();
            builder.RegisterType<KeePassPasswordProvider>().AsImplementedInterfaces().SingleInstance();

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
