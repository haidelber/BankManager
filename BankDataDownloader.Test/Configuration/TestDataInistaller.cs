using System.Data.Entity;
using Autofac;
using BankDataDownloader.Data;
using BankDataDownloader.Data.Configuration;
using SQLite.CodeFirst;

namespace DataDownloader.Test.Configuration
{
    public class TestDataInistaller : DataInstallerBase
    {
        protected override void RegisterContext(ContainerBuilder cb)
        {
            cb.RegisterType<DataContext>().As<DbContext>();
            //TODO this is dangerous as it kills all the data only do this while testing! should use migrations one day
            //https://github.com/msallin/SQLiteCodeFirst/issues/4
            cb.RegisterType<SqliteDropCreateDatabaseAlways<DataContext>>().AsImplementedInterfaces();
        }
    }
}