using System.Collections.Generic;
using BankManager.Core.Model.Porfolio;
using BankManager.Core.Model.Transaction;

namespace BankManager.Core.Service
{
    public interface IPortfolioService
    {
        IEnumerable<PortfolioGroupModel> PortfolioGroups();
        IEnumerable<PortfolioPositionModel> PortfolioGroupPositions(long portfolioGroupId);
        PortfolioGroupModel GetPortfolioGroup(long portfolioGroupId);
        PortfolioGroupModel CreateEditPortfolioGroup(PortfolioGroupModel model);
    }
}