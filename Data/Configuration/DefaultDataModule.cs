using Autofac;
using Microsoft.EntityFrameworkCore;

namespace BankManager.Data.Configuration
{
    public class DefaultDataModule : DataModuleBase
    {
        protected override void RegisterContext(ContainerBuilder builder)
        {
            builder.RegisterType<DataContext>().As<DbContext>().InstancePerLifetimeScope();
        }
    }
}
