using System.Reflection;
using System.Linq;
using Autofac;
using BankManager.Common.Extensions;
using BankManager.Data.Repository.Impl;
using Module = Autofac.Module;

namespace BankManager.Data.Configuration
{
    public abstract class DataModuleBase : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var data = Assembly.GetExecutingAssembly();

            //Registers generic repositories
            var alltypes = data.GetTypes();
            var genericRepositories = alltypes
                    .Where(t => t.IsGenericType && !t.IsInterface && !t.IsAbstract && t.IsAssignableToGenericType(typeof(Repository<>)));
            foreach (var genericRepository in genericRepositories)
            {
                builder.RegisterGeneric(genericRepository).AsImplementedInterfaces().PropertiesAutowired();
            }
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
