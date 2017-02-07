using Autofac;

namespace BankDataDownloader.Common.Configuration
{
    public interface IInstaller
    {
        void RegisterComponents(ContainerBuilder cb);
    }
}
