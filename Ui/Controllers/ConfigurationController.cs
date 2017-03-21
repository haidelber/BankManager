using AutoMapper;
using BankDataDownloader.Common.Model.Configuration;
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

        [HttpGet]
        public ApplicationConfigurationModel Get()
        {
            return Mapper.Map<ApplicationConfigurationModel>(ConfigurationService.ApplicationConfiguration);
        }

        [HttpGet("[action]")]
        public string ConfigurationFilePath()
        {
            return ConfigurationService.ConfigurationFilePath;
        }

        [HttpPost("[action]")]
        public void GenerateConfigFile()
        {
            ConfigurationService.SaveConfiguration(ConfigurationService.ApplicationConfiguration);
        }

        [HttpPost("[action]")]
        public ApplicationConfigurationModel Save([FromBody]ApplicationConfigurationModel newModel)
        {
            var newConfig = Mapper.Map<ApplicationConfiguration>(newModel);
            ConfigurationService.SaveConfiguration(newConfig);
            return Get();
        }
    }
}
