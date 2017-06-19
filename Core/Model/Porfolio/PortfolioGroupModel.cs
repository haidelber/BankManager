using System.Collections.Generic;

namespace BankManager.Core.Model.Porfolio
{
    public class PortfolioGroupModel
    {
        public long Id { get; set; }

        public string Name { get; set; }
        public IEnumerable<string> AssignedIsins { get; set; }

        public decimal TargetPercentage { get; set; }

        public bool IncludeInCalculations { get; set; }
    }
}
