using Autofac;
using BankDataDownloader.Core.Configuration;

namespace DataDownloader.Test.Configuration
{
    public class TestConfiguration
    {
        public ContainerBuilder Builder { get; }

        private IContainer _container;

        public IContainer Container => _container ?? (_container = Builder.Build());

        public TestConfiguration()
        {
            Builder = new ContainerBuilder();
            Register();
        }

        private void Register()
        {
            new ServiceInstaller().RegisterComponents(Builder);
        }
    }
}
