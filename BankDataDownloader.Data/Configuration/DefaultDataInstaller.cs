using System.Data.Entity;
using System.Reflection;
using Autofac;
using BankDataDownloader.Common.Configuration;
using SQLite.CodeFirst;

namespace BankDataDownloader.Data.Configuration
{
    public class DefaultDataInstaller : DataInstallerBase
    {
        protected override void RegisterContext(ContainerBuilder cb)
        {
            cb.RegisterType<DataContext>().As<DbContext>();
            cb.RegisterType<SqliteCreateDatabaseIfNotExists<DataContext>>().AsImplementedInterfaces();
        }
    }
}
