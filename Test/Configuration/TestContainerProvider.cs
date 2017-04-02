using System;
using Autofac;
using BankManager.Core.Configuration;
using BankManager.Data.Configuration;

namespace BankManager.Test.Configuration
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
            Builder.RegisterModule<DefaultServiceModule>();
            Builder.RegisterModule<DefaultDataModule>();
            //Builder.RegisterModule<TestDataModule>();
        }

        public void Dispose()
        {
            _container.Dispose();
        }
    }
}
