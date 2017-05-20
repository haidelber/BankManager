using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BankManager.Core.Model.Porfolio;
using BankManager.Core.Model.Transaction;
using BankManager.Data.Entity;
using BankManager.Data.Repository;

namespace BankManager.Core.Service.Impl
{
    public class PortfolioService : IPortfolioService
    {
        public IRepository<PortfolioGroupEntity> PortfolioGroupRepository { get; }
        public IPortfolioPositionRepository PortfolioPositionRepository { get; }

        public PortfolioService(IRepository<PortfolioGroupEntity> portfolioGroupRepository, IPortfolioPositionRepository portfolioPositionRepository)
        {
            PortfolioGroupRepository = portfolioGroupRepository;
            PortfolioPositionRepository = portfolioPositionRepository;
        }

        public IEnumerable<PortfolioGroupModel> PortfolioGroups()
        {
            return Mapper.Map<IEnumerable<PortfolioGroupModel>>(PortfolioGroupRepository.GetAll()
                .OrderBy(entity => entity.Name));
        }

        public IEnumerable<PortfolioPositionModel> PortfolioGroupPositions(long portfolioGroupId)
        {
            var isins = PortfolioGroupRepository.GetById(portfolioGroupId).AssignedIsins.ToList();
            var positions =
                PortfolioPositionRepository.Query().Where(entity => isins.Contains(entity.Isin))
                    .GroupBy(entity => new { entity.Isin, entity.Portfolio })
                    .Select(entities => entities.OrderByDescending(entity => entity.DateTime).FirstOrDefault())
                    .ToList();
            return Mapper.Map<IEnumerable<PortfolioPositionModel>>(positions);
        }

        public PortfolioGroupModel GetPortfolioGroup(long portfolioGroupId)
        {
            return Mapper.Map<PortfolioGroupModel>(PortfolioGroupRepository.GetById(portfolioGroupId));
        }

        public PortfolioGroupModel CreateEditPortfolioGroup(PortfolioGroupModel model)
        {
            PortfolioGroupEntity entity;
            //is update
            if (model.Id != default(long))
            {
                entity = PortfolioGroupRepository.GetById(model.Id);
                entity = Mapper.Map(model, entity);
            }
            else
            {
                entity = Mapper.Map<PortfolioGroupEntity>(model);
                entity = PortfolioGroupRepository.Insert(entity);
            }
            PortfolioGroupRepository.Save();
            return Mapper.Map<PortfolioGroupModel>(entity);
        }
    }
}
