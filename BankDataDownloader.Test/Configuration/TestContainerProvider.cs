using System;
using Autofac;
using BankDataDownloader.Core.Configuration;
using BankDataDownloader.Data.Configuration;

namespace DataDownloader.Test.Configuration
{
    public class TestContainerProvider : IDisposable
    {
        public ContainerBuilder Builder { get; }

        private IContainer _container;

        public IContainer Container => _container ?? (_container = Builder.Build());

        public TestContainerProvider()
        {
            Builder = new ContainerBuilder();
            Register();
        }

        private void Register()
        {
            new DefaultServiceInstaller().RegisterComponents(Builder);
            new TestDataInistaller().RegisterComponents(Builder);
            //new DefaultDataInstaller().RegisterComponents(Builder);
        }

        public void Dispose()
        {
            _container.Dispose();
        }
    }
}
