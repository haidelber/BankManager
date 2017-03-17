using AutoMapper;
using BankDataDownloader.Core.Service;
using BankDataDownloader.Ui.Model.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace BankDataDownloader.Ui.Controllers
{
    [Route("api/[controller]")]
    public class ConfigurationController:Controller
    {
        public IConfigurationService ConfigurationService { get; }
        public IMapper Mapper { get;  }

        public ConfigurationController(IConfigurationService configurationService, IMapper mapper)
        {
            ConfigurationService = configurationService;
            Mapper = mapper;
        }
        
        public ApplicationConfigurationModel ApplicationConfiguration()
        {
            return Mapper.Map<ApplicationConfigurationModel>(ConfigurationService.ApplicationConfiguration);
        }
    }
}
