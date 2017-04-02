using System.Linq;
using BankManager.Common.Extensions;
using BankManager.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace BankManager.Data.Repository.Impl
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