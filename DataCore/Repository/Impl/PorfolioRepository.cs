using System.Linq;
using BankDataDownloader.Common.Extensions;
using BankDataDownloader.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace BankDataDownloader.Data.Repository.Impl
{
    public class PorfolioRepository : Repository<PortfolioEntity>, IPortfolioRepository
    {
        public PorfolioRepository(DbContext context) : base(context)
        {
        }

        public PortfolioEntity GetByPortfolioNumberAndBankName(string portfolioNumber, string bankName)
        {
            var cleanPortfoliNumber = portfolioNumber.CleanString();
            return Query().SingleOrDefault(entity => entity.PortfolioNumber == cleanPortfoliNumber && entity.BankName == bankName);
        }
    }
}