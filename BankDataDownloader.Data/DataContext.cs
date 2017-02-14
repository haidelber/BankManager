using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SQLite;
using Autofac;
using BankDataDownloader.Common.Model.Configuration;
using BankDataDownloader.Data.Entity;

namespace BankDataDownloader.Data
{
    public class DataContext : DbContext
    {
        public new DatabaseConfiguration Configuration { get; }
        public IComponentContext Context { get; }

        public DbSet<BankTransactionEntity> BankTransactions { get; set; }
        public DbSet<AccountEntity> AccountEntities { get; set; }

        public DataContext(DatabaseConfiguration configuration, IComponentContext context) :
            base(new SQLiteConnection
            {
                ConnectionString = new SQLiteConnectionStringBuilder { DataSource = configuration.DatabasePath, ForeignKeys = true }.ConnectionString
            }, true)
        {
            Configuration = configuration;
            Context = context;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingEntitySetNameConvention>();

            var sqliteConnectionInitializerFromContext =
                Context.Resolve<IDatabaseInitializer<DataContext>>(new TypedParameter(typeof(DbModelBuilder),
                    modelBuilder));
            //var sqliteConnectionInitializer = new SqliteDropCreateDatabaseWhenModelChanges<DataContext>(modelBuilder);
            Database.SetInitializer(sqliteConnectionInitializerFromContext);

            modelBuilder.Entity<AccountEntity>()
                .HasMany(entity => entity.Transactions)
                .WithRequired(entity => entity.Account);
        }
    }
}
