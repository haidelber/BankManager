using System.Data.Entity;
using Autofac;
using SQLite.CodeFirst;

namespace BankDataDownloader.Data.Configuration
{
    public class DefaultDataModule : DataModuleBase
    {
        protected override void RegisterContext(ContainerBuilder builder)
        {
            builder.RegisterType<DataContext>().As<DbContext>();
            builder.RegisterType<SqliteCreateDatabaseIfNotExists<DataContext>>().AsImplementedInterfaces();
        }
    }
}
