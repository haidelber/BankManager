using System.Linq;
using BankManager.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace BankManager.Data.Repository.Impl
{
    public class PortfolioPositionRepository : Repository<PositionEntity>, IPortfolioPositionRepository
    {
        public PortfolioPositionRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public IQueryable<PositionEntity> GetAllByPortfolioId(long id)
        {
            return Query().Include(entity => entity.Portfolio).Where(entity => entity.Portfolio.Id == id);
        }
    }
}