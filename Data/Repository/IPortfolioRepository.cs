using BankManager.Data.Entity;

namespace BankManager.Data.Repository
{
    public interface IPortfolioRepository : IRepository<PortfolioEntity>
    {
        PortfolioEntity GetByPortfolioNumberAndBankName(string portfolioNumber, string bankName);
    }
}