using System.Reflection;
using Autofac;
using BankDataDownloader.Data.Repository.Impl;
using Module = Autofac.Module;

namespace BankDataDownloader.Data.Configuration
{
    public abstract class DataModuleBase : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var data = Assembly.GetExecutingAssembly();

            //Registers default repositories
            builder.RegisterGeneric(typeof(Repository<>)).AsImplementedInterfaces().PropertiesAutowired();
            //Registers specifically written repositories
            builder.RegisterAssemblyTypes(data).Where(t => t.Name.EndsWith("Repository")).AsImplementedInterfaces()
                .PropertiesAutowired();

            builder.RegisterAssemblyTypes(data)
                .AsClosedTypesOf(typeof(IEntityEqualityComparer<>))
                .AsImplementedInterfaces();

            RegisterContext(builder);
        }

        protected abstract void RegisterContext(ContainerBuilder builder);
    }
}
