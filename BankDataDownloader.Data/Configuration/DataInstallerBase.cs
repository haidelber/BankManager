using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace BankDataDownloader.Data.Configuration
{
    public abstract class DataInstallerBase
    {
        public void RegisterComponents(ContainerBuilder cb)
        {
            RegisterRepositories(cb);
            RegisterContext(cb);
        }

        protected void RegisterRepositories(ContainerBuilder cb)
        {
            var data = Assembly.GetExecutingAssembly();
            cb.RegisterAssemblyTypes(data)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces();
        }

        protected abstract void RegisterContext(ContainerBuilder cb);
    }
}
