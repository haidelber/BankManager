using Autofac;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace BankManager.Data.Configuration
{
    public class DefaultDataModule : DataModuleBase
    {
        public readonly Logger Logger = LogManager.GetCurrentClassLogger();

        protected override void RegisterContext(ContainerBuilder builder)
        {
            Logger.Info($"Registering {GetType().Name}..");

            builder.RegisterType<DataContext>().As<DbContext>().InstancePerLifetimeScope();
        }
    }
}
