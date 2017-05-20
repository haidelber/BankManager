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
            typeof(PositionEntity).Assembly.GetTypes()
                .Where(type => typeof(PositionEntity).IsAssignableFrom(type))
                .ToList()
                .ForEach(type => { CreateMap(type, typeof(PortfolioPositionModel)).ReverseMap(); });
            typeof(TransactionEntity).Assembly.GetTypes()
                .Where(type => typeof(TransactionEntity).IsAssignableFrom(type) &&
                               !typeof(TransactionForeignCurrencyEntity).IsAssignableFrom(type)).ToList()
                .ForEach(type => { CreateMap(type, typeof(BankTransactionModel)).ReverseMap(); });
            typeof(TransactionForeignCurrencyEntity).Assembly.GetTypes()
                .Where(type => typeof(TransactionForeignCurrencyEntity).IsAssignableFrom(type)).ToList()
                .ForEach(type => { CreateMap(type, typeof(BankTransactionForeignCurrencyModel)).ReverseMap(); });
        }
    }

    public class PortfolioProfile : Profile
    {
        public PortfolioProfile()
        {
            CreateMap<PortfolioGroupEntity, PortfolioGroupEntity>().ReverseMap();
        }
    }
}
