using System.Linq;
using BankDataDownloader.Data.Entity;

namespace BankDataDownloader.Data.Repository
{
    public interface IPortfolioPositionRepository : IRepository<PositionEntity>
    {
        IQueryable<PositionEntity> GetAllByPortfolioId(long id);
    }
}