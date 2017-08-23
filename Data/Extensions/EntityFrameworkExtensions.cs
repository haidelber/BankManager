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
            var name = t.Name.Replace("Entity", "");
            foreach (var entity in assembly.GetTypes().Where(t.IsAssignableFrom))
            {
                
                modelBuilder.Entity(entity).ToTable(name);
            }
            return modelBuilder;
        }
    }
}
