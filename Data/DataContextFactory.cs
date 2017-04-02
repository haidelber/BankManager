using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankManager.Common;
using BankManager.Common.Model.Configuration;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace BankManager.Data
{
    public class DataContextFactory: IDbContextFactory<DataContext>
    {
        public DataContext Create(DbContextFactoryOptions options)
        {
            return new DataContext(new DatabaseConfiguration {DatabasePath = TestConstants.Data.DatabasePath});
        }
    }
}
