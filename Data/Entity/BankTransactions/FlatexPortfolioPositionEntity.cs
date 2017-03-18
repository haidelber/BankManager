using System;

namespace BankDataDownloader.Data.Entity.BankTransactions
{
    public class FlatexPortfolioPositionEntity : PortfolioPositionEntity,
        IEntityEqualityComparer<FlatexPortfolioPositionEntity>
    {
        public Func<FlatexPortfolioPositionEntity, bool> Func(FlatexPortfolioPositionEntity otherEntity)
        {
            throw new NotImplementedException();
        }
    }
}