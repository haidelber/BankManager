using System;
using System.ComponentModel.DataAnnotations;
using SQLite.CodeFirst;

namespace BankDataDownloader.Data.Entity
{
    public class EntityBase : IEntityEqualityComparer<EntityBase>
    {
        [Key]
        public long Id { get; set; }

        public Func<EntityBase, bool> Func(EntityBase otherEntity)
        {
            return entity => entity.Id == otherEntity.Id;
        }
    }
}
