using System;
using Autofac;
using BankManager.Core.Configuration;
using BankManager.Data.Configuration;

namespace BankManager.Test._Configuration
{
    public class TestContainerProvider : IDisposable
    {
        public ContainerBuilder Builder { get; }

        private IContainer _container;

        public IContainer Container => _container ?? (_container = Builder.Build());

        public TestContainerProvider()
        {
            Builder = new ContainerBuilder();
        }

        private void Register()
        {
            
        }

        public void Dispose()
        {
            _container.Dispose();
        }
    }
}
