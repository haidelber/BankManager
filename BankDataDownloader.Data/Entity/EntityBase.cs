using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BankDataDownloader.Data.Entity
{
    public class EntityBase : IEntityEqualityComparer<EntityBase>
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Func<EntityBase, bool> Func(EntityBase otherEntity)
        {
            return entity => entity.Id == otherEntity.Id;
        }
    }
}
