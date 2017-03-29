using System.Linq;
using AutoMapper;
using BankDataDownloader.Common.Model.Configuration;
using BankDataDownloader.Core.Model.Account;
using BankDataDownloader.Core.Model.Configuration;
using BankDataDownloader.Core.Model.Transaction;
using BankDataDownloader.Data.Entity;

namespace BankDataDownloader.Core.Helper.Automapper
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
            CreateMap<ApplicationConfiguration, ApplicationConfigurationModel>();
            //TODO create remaining type maps and remove CreateMissingTypeMaps in AutoMapperModule
        }
    }

    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<BankAccountEntity, BankAccountModel>().ReverseMap();
            CreateMap<CreditCardAccountEntity, CreditCardAccountModel>().ReverseMap();
            CreateMap<PortfolioEntity, PortfolioModel>().ReverseMap();
        }
    }

    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            typeof(PortfolioPositionEntity).Assembly.GetTypes()
                .Where(type => typeof(PortfolioPositionEntity).IsAssignableFrom(type))
                .ToList()
                .ForEach(type => { CreateMap(type, typeof(PortfolioPositionModel)); });
            typeof(BankTransactionEntity).Assembly.GetTypes()
                .Where(type => typeof(BankTransactionEntity).IsAssignableFrom(type) &&
                               !typeof(BankTransactionForeignCurrencyEntity).IsAssignableFrom(type)).ToList()
                .ForEach(type => { CreateMap(type, typeof(BankTransactionModel)); });
            typeof(BankTransactionForeignCurrencyEntity).Assembly.GetTypes()
                .Where(type => typeof(BankTransactionForeignCurrencyEntity).IsAssignableFrom(type)).ToList()
                .ForEach(type => { CreateMap(type, typeof(BankTransactionForeignCurrencyModel)); });
        }
    }
}
