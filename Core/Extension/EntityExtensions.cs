using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankDataDownloader.Data.Entity;

namespace BankDataDownloader.Core.Extension
{
    public static class EntityExtensions
    {
        public static bool HasValidId<T>(this T entity) where T : EntityBase
        {
            return entity.Id != default(long);
        }
    }
}
