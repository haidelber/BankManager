using System.Reflection;
using Autofac;
using BankDataDownloader.Common.Configuration;
using BankDataDownloader.Core.Service.Impl;

namespace BankDataDownloader.Core.Configuration
{
    public class ServiceInstaller : IInstaller
    {
        public void RegisterComponents(ContainerBuilder cb)
        {
            var core = Assembly.GetExecutingAssembly();

            cb.RegisterAssemblyTypes(core)
                   .Where(t => t.Name.EndsWith("DownloadHandler"))
                   .AsSelf();
            cb.RegisterAssemblyTypes(core).Where(t => t.Name.EndsWith("Service")).Except<ConfigurationService>();
        }
    }
}
