using System.Linq;
using AutoMapper;
using BankManager.Common.Model.Configuration;
using BankManager.Core.Model.Account;
using BankManager.Core.Model.Configuration;
using BankManager.Core.Model.Porfolio;
using BankManager.Core.Model.Transaction;
using BankManager.Data.Entity;

namespace BankManager.Core.Helper.Automapper
{
    public class DownloadHandlerModelProfile : Profile
    {
        public DownloadHandlerModelProfile()
        {

        }
    }

    public class ConfigurationModelProfile : Profile
    {
        public ConfigurationModelProfile()
        {
            CreateMap<KeePassConfiguration, KeePassConfigurationModel>().ReverseMap();
            CreateMap<DatabaseConfiguration, DatabaseConfigurationModel>().ReverseMap();
            CreateMap<DownloadHandlerConfiguration, DownloadHandlerConfigurationModel>().ReverseMap();
            //TODO create remaining type maps and remove CreateMissingTypeMaps in AutoMapperModule
        }
    }

    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<BankAccountEntity, BankAccountModel>().ReverseMap();
            CreateMap<CreditCardEntity, CreditCardAccountModel>().ReverseMap();
            CreateMap<PortfolioEntity, PortfolioModel>().ReverseMap();
        }
    }

    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            var positionTypes = typeof(PositionEntity).Assembly.GetTypes()
                .Where(type => typeof(PositionEntity).IsAssignableFrom(type))
                .ToList();
                positionTypes.ForEach(type => { CreateMap(type, typeof(PortfolioPositionModel)).ReverseMap(); });
                positionTypes.ForEach(type => { CreateMap(type, typeof(CumulativePositionModel)).ReverseMap(); });

            var transactionTypes = typeof(TransactionEntity).Assembly.GetTypes()
                .Where(type => typeof(TransactionEntity).IsAssignableFrom(type) &&
                               !typeof(TransactionForeignCurrencyEntity).IsAssignableFrom(type)).ToList();
                transactionTypes.ForEach(type => { CreateMap(type, typeof(BankTransactionModel)).ReverseMap(); });
                transactionTypes.ForEach(type => { CreateMap(type, typeof(CumulativeTransactionModel)).ReverseMap(); });

            typeof(TransactionForeignCurrencyEntity).Assembly.GetTypes()
                .Where(type => typeof(TransactionForeignCurrencyEntity).IsAssignableFrom(type)).ToList()
                .ForEach(type => { CreateMap(type, typeof(BankTransactionForeignCurrencyModel)).ReverseMap(); });
        }
    }

    public class PortfolioProfile : Profile
    {
        public PortfolioProfile()
        {
            CreateMap<PortfolioGroupEntity, PortfolioGroupModel>().ForMember(x => x.AssignedIsins,
                opt => opt.MapFrom(src => src.AssignedIsinList.Split(';')));
            CreateMap<PortfolioGroupModel, PortfolioGroupEntity>().ForMember(x => x.AssignedIsinList,
                opt => opt.MapFrom(src => string.Join(";", src.AssignedIsins)));
        }
    }
}
