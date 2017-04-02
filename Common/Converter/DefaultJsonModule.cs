using System.Reflection;
using Autofac;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog;
using Module = Autofac.Module;

namespace BankManager.Common.Converter
{
    public class DefaultJsonModule : Module
    {
        public readonly Logger Logger = LogManager.GetCurrentClassLogger();

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            Logger.Info($"Registering {GetType().Name}..");

            var common = Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(common)
                .Where(t => t.IsSubclassOf(typeof(JsonConverter))).AsSelf().InstancePerDependency();
            builder.RegisterType<CustomContractResolver>().As<IContractResolver>().InstancePerDependency();
            builder.RegisterType<CustomSerializer>().As<JsonSerializer>().AsSelf().InstancePerDependency();
        }
    }
}
