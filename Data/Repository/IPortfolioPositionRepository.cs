using System.Linq;
using BankDataDownloader.Data.Entity;

namespace BankDataDownloader.Data.Repository
{
    public interface IPortfolioPositionRepository : IRepository<PortfolioPositionEntity>
    {
        IQueryable<PortfolioPositionEntity> GetAllByPortfolioId(long id);
    }
}