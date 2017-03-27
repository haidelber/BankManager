using System.Data.Entity;
using System.Linq;
using BankDataDownloader.Data.Entity;

namespace BankDataDownloader.Data.Repository.Impl
{
    public class PortfolioPositionRepository : Repository<PortfolioPositionEntity>, IPortfolioPositionRepository
    {
        public PortfolioPositionRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public IQueryable<PortfolioPositionEntity> GetAllByPortfolioId(long id)
        {
            return Query().Where(entity => entity.Portfolio.Id == id);
        }
    }
}