using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Module = Autofac.Module;

namespace BankDataDownloader.Data.Configuration
{
    public abstract class DataModuleBase : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterRepositories(builder);
            RegisterContext(builder);
        }

        protected void RegisterRepositories(ContainerBuilder builder)
        {
            var data = Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(data)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces();
        }

        protected abstract void RegisterContext(ContainerBuilder builder);
    }
}
