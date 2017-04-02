using System.Linq;
using BankManager.Data.Entity;

namespace BankManager.Data.Repository
{
    public interface IPortfolioPositionRepository : IRepository<PositionEntity>
    {
        IQueryable<PositionEntity> GetAllByPortfolioId(long id);
    }
}