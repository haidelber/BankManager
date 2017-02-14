using System;

namespace BankDataDownloader.Data.Entity
{
    public class PortfolioEntity : AccountEntity, IEntityEqualityComparer<PortfolioEntity>
    {
        public string PortfolioNumber { get; set; }

        public Func<PortfolioEntity, bool> Func(PortfolioEntity otherEntity)
        {
            return entity => entity.BankName == otherEntity.BankName && entity.PortfolioNumber == otherEntity.PortfolioNumber;
        }
    }
}