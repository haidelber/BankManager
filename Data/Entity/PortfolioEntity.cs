﻿using System;
using System.Collections.Generic;

namespace BankManager.Data.Entity
{
    public class PortfolioEntity : EntityBase, IEntityEqualityComparer<PortfolioEntity>
    {
        public string BankName { get; set; }
        public string AccountName { get; set; }
        public bool Active { get; set; }

        public string PortfolioNumber { get; set; }

        public virtual ICollection<PositionEntity> Positions { get; set; }

        public PortfolioEntity()
        {
            Active = true;
        }

        public Func<PortfolioEntity, bool> Func(PortfolioEntity otherEntity)
        {
            return entity => entity.BankName == otherEntity.BankName && entity.PortfolioNumber == otherEntity.PortfolioNumber;
        }
    }
}