﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankManager.Data.Entity
{
    public class PortfolioGroupEntity : EntityBase
    {
        public string Name { get; set; }
        public virtual ICollection<string> AssignedIsins { get; set; }

        public decimal LowerThresholdPercentage { get; set; }
        public decimal TargetPercentage { get; set; }
        public decimal UpperThresholdPercentage { get; set; }
    }
}
