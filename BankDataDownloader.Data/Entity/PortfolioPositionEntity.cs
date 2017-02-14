using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankDataDownloader.Data.Entity
{
    public abstract class PortfolioPositionEntity : EntityBase
    {
        public string Isin { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateTime { get; set; }

        public decimal CurrentValue { get; set; }
        public string CurrentValueCurrencyIso { get; set; }

        public decimal OriginalValue { get; set; }
        public string OriginalValueCurrencyIso { get; set; }

        public virtual PortfolioEntity Portfolio { get; set; }
    }
}
