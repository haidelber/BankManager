using System.Linq;
using System.Reflection;
using AutoMapper;
using BankDataDownloader.Common.Model.Configuration;
using BankDataDownloader.Data.Entity;
using BankManager.Ui.Model.Account;
using BankManager.Ui.Model.Configuration;
using BankManager.Ui.Model.Transaction;

namespace BankManager.Ui.Helper.Automapper
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
            CreateMap<BankAccountEntity, BankAccountModel>();
            CreateMap<CreditCardAccountEntity, CreditCardAccountModel>();
            CreateMap<PortfolioEntity, PortfolioModel>();
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
