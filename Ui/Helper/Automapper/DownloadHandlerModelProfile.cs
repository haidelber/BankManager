using AutoMapper;
using BankDataDownloader.Common.Model.Configuration;
using BankDataDownloader.Ui.Model.Configuration;

namespace BankDataDownloader.Ui.Helper.Automapper
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
}
