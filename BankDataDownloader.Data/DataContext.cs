using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using BankDataDownloader.Common.Model.Configuration;
using BankDataDownloader.Data.Configuration;
using BankDataDownloader.Data.Entity;
using SQLite.CodeFirst;

namespace BankDataDownloader.Data
{
    public class DataContext : DbContext
    {
        public new DatabaseConfiguration Configuration { get; }
        public IContainer Container { get; }

        public DbSet<RaiffeisenTransaction> RaiffeisenTransactions { get; set; }

        public DataContext(DatabaseConfiguration configuration, IContainer container) :
            base(new SQLiteConnection
            {
                ConnectionString = new SQLiteConnectionStringBuilder { DataSource = configuration.DatabasePath, ForeignKeys = true }.ConnectionString
            }, true)
        {
            Configuration = configuration;
            Container = container;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var sqliteConnectionInitializerFromContext =
                Container.Resolve<IDatabaseInitializer<DataContext>>(new TypedParameter(typeof(DbModelBuilder),
                    modelBuilder));
            //var sqliteConnectionInitializer = new SqliteDropCreateDatabaseWhenModelChanges<DataContext>(modelBuilder);
            Database.SetInitializer(sqliteConnectionInitializerFromContext);
        }
    }
}
