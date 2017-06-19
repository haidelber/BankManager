using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankManager.Data.Entity
{
    public class PortfolioGroupEntity : EntityBase
    {
        public string Name { get; set; }
        public virtual string AssignedIsinList { get; set; }

        public decimal TargetPercentage { get; set; }

        public bool IncludeInCalculations { get; set; }
    }
}
