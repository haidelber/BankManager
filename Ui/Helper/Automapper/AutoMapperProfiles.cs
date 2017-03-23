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
            var portfolioTypes = typeof(PortfolioPositionEntity).Assembly.GetTypes()
                .Where(type => type.IsSubclassOf(typeof(PortfolioPositionEntity)));
            portfolioTypes.Aggregate(
                CreateMap<PortfolioPositionEntity, PortfolioPositionModel>(),
                (current, type) => current.Include(type, typeof(PortfolioPositionModel)));

            var bankTypes =
                typeof(BankTransactionEntity).Assembly.GetTypes()
                    .Where(type => type.IsSubclassOf(typeof(BankTransactionEntity)) &&
                                   !type.IsAssignableFrom(typeof(BankTransactionForeignCurrencyEntity)));
            var foreignTypes = typeof(BankTransactionForeignCurrencyEntity).Assembly.GetTypes()
                .Where(type => type.IsAssignableFrom(typeof(BankTransactionForeignCurrencyEntity)));
            
            foreignTypes.Aggregate(
                bankTypes.Aggregate(
                    CreateMap<BankTransactionEntity, BankTransactionModel>(),
                    (current, type) => current.Include(type, typeof(BankTransactionModel))),
                (current, type) => current.Include(type, typeof(BankTransactionForeignCurrencyModel)));
        }
    }
}
