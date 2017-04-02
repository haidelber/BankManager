using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BankManager.Data.Extensions
{
    public static class EntityFrameworkExtensions
    {
        public static ModelBuilder RegisterSubtypes(this ModelBuilder modelBuilder, Type t)
        {
            var assembly = Assembly.GetExecutingAssembly();
            foreach (var entity in assembly.GetTypes().Where(t.IsAssignableFrom))
            {
                modelBuilder.Entity(entity).ForSqliteToTable(t.Name.Replace("Entity", ""));
            }
            return modelBuilder;
        }
    }
}
