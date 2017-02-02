using System.Reflection;
using Autofac;
using BankDataDownloader.Common.Installer;
using BankDataDownloader.Core.DownloadHandler.Interfaces;
using BankDataDownloader.Core.KeePass;

namespace BankDataDownloader.Core.Installer
{
    public class ServiceInstaller : IInstaller
    {
        public void RegisterComponents(ContainerBuilder cb)
        {
            var core = Assembly.GetExecutingAssembly();

            cb.RegisterAssemblyTypes(core)
                   .Where(t => t.Name.EndsWith("DownloadHandler"))
                   .AsSelf();
        }
    }
}
