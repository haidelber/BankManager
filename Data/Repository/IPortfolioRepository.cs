using BankDataDownloader.Data.Entity;

namespace BankDataDownloader.Data.Repository
{
    public interface IPortfolioRepository : IRepository<PortfolioEntity>
    {
        PortfolioEntity GetByPortfolioNumberAndBankName(string portfolioNumber, string bankName);
    }
}