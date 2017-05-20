using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BankManager.Core.Model.Porfolio;
using BankManager.Core.Model.Transaction;
using BankManager.Data.Entity;
using BankManager.Data.Repository;
using Microsoft.EntityFrameworkCore;

namespace BankManager.Core.Service.Impl
{
    public class PortfolioService : IPortfolioService
    {
        public IRepository<PortfolioGroupEntity> PortfolioGroupRepository { get; }
        public IPortfolioPositionRepository PortfolioPositionRepository { get; }
        public IMapper Mapper { get; }

        public PortfolioService(IRepository<PortfolioGroupEntity> portfolioGroupRepository, IPortfolioPositionRepository portfolioPositionRepository, IMapper mapper)
        {
            PortfolioGroupRepository = portfolioGroupRepository;
            PortfolioPositionRepository = portfolioPositionRepository;
            Mapper = mapper;
        }

        public IEnumerable<PortfolioGroupModel> PortfolioGroups()
        {
            return Mapper.Map<IEnumerable<PortfolioGroupModel>>(PortfolioGroupRepository.GetAll()
                .OrderBy(entity => entity.Name));
        }

        public IEnumerable<PortfolioPositionModel> PortfolioGroupPositions(long portfolioGroupId)
        {
            var isins = GetPortfolioGroup(portfolioGroupId).AssignedIsins.ToList();
            var positions =
                PortfolioPositionRepository.Query().Where(model => isins.Contains(model.Isin))
                    .Include(entity => entity.Portfolio)
                    .GroupBy(entity => new { entity.Isin, entity.Portfolio })
                    .Select(entities => entities.OrderByDescending(entity => entity.DateTime).FirstOrDefault())
                    .ToList();
            //TODO contains ignore case
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
