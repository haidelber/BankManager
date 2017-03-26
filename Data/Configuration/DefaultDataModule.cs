using System.Data.Entity;
using Autofac;
using NLog;
using SQLite.CodeFirst;

namespace BankDataDownloader.Data.Configuration
{
    public class DefaultDataModule : DataModuleBase
    {
        public readonly Logger Logger = LogManager.GetCurrentClassLogger();

        protected override void RegisterContext(ContainerBuilder builder)
        {
            Logger.Info($"Registering {GetType().Name}..");

            builder.RegisterType<DataContext>().As<DbContext>().InstancePerLifetimeScope();
            builder.RegisterType<SqliteCreateDatabaseIfNotExists<DataContext>>().AsImplementedInterfaces();
        }
    }
}
