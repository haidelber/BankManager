using BankManager.Common.Model.Configuration;
using BankManager.Data.Entity;
using BankManager.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace BankManager.Data
{
    public class DataContext : DbContext
    {
        public DatabaseConfiguration Configuration { get; }

        /// <summary>
        /// Called in normal production.
        /// </summary>
        /// <param name="configuration"></param>
        public DataContext(DatabaseConfiguration configuration) : base()
        {
            Configuration = configuration;
        }
        /// <summary>
        /// Called from tests with given inmemory options.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="configuration"></param>
        public DataContext(DbContextOptions<DataContext> options, DatabaseConfiguration configuration)
        : base(options)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite($"Data Source={Configuration.DatabasePath}");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.RegisterSubtypes(typeof(PortfolioEntity));
            modelBuilder.RegisterSubtypes(typeof(PositionEntity));
            modelBuilder.RegisterSubtypes(typeof(AccountEntity));
            modelBuilder.RegisterSubtypes(typeof(TransactionEntity));

            modelBuilder.Entity<AccountEntity>().HasMany(entity => entity.Transactions).WithOne(entity => entity.Account);
            modelBuilder.Entity<PortfolioEntity>().HasMany(entity => entity.Positions).WithOne(entity => entity.Portfolio);
        }


    }
}
