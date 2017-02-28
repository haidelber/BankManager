using System.Data.Entity;
using Autofac;
using BankDataDownloader.Data;
using BankDataDownloader.Data.Configuration;
using SQLite.CodeFirst;

namespace BankDataDownloader.Test.Configuration
{
    public class TestDataModule : DataModuleBase
    {
        protected override void RegisterContext(ContainerBuilder builder)
        {
            builder.RegisterType<DataContext>().As<DbContext>().SingleInstance();
            //TODO this is dangerous as it kills all the data only do this while testing! should use migrations one day
            //https://github.com/msallin/SQLiteCodeFirst/issues/4
            builder.RegisterType<SqliteDropCreateDatabaseAlways<DataContext>>().AsImplementedInterfaces();
            //builder.RegisterType<SqliteCreateDatabaseIfNotExists<DataContext>>().AsImplementedInterfaces();
        }
    }
}