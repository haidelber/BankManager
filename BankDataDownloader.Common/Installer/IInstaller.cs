using Autofac;

namespace BankDataDownloader.Common.Installer
{
    public interface IInstaller
    {
        void RegisterComponents(ContainerBuilder cb);
    }
}
