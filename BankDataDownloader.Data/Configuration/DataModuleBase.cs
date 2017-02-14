using System.Linq;
using System.Reflection;
using Autofac;
using BankDataDownloader.Data.Repository;
using Module = Autofac.Module;

namespace BankDataDownloader.Data.Configuration
{
    public abstract class DataModuleBase : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var data = Assembly.GetExecutingAssembly();
            builder.RegisterGeneric(typeof(Repository<>)).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(data)
                .Where(t => t.Name.EndsWith("Repository")).PropertiesAutowired()
                .AsImplementedInterfaces();

            //TODO check why not resolving
            builder.RegisterAssemblyTypes(data)
                .Where(t => t.GetInterfaces().Any(x => x == typeof(IEntityEqualityComparer<>)))
                .AsImplementedInterfaces();
            RegisterContext(builder);
        }

        protected abstract void RegisterContext(ContainerBuilder builder);
    }
}
