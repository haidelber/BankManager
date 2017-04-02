using AutoMapper;
using BankManager.Common.Model.Configuration;
using BankManager.Core.Model.Configuration;
using BankManager.Core.Service;
using Microsoft.AspNetCore.Mvc;

namespace BankManager.Ui.Controllers
{
  public class ConfigurationController:ApiController
    {
        public IConfigurationService ConfigurationService { get; }
        public IMapper Mapper { get;  }

        public ConfigurationController(IConfigurationService configurationService, IMapper mapper)
        {
            ConfigurationService = configurationService;
            Mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Json(Mapper.Map<ApplicationConfigurationModel>(ConfigurationService.ApplicationConfiguration));
        }

        [HttpGet("[action]")]
        public IActionResult ConfigurationFilePath()
        {
            return Json(ConfigurationService.ConfigurationFilePath);
        }

        [HttpPost("[action]")]
        public IActionResult GenerateConfigFile()
        {
            ConfigurationService.SaveConfiguration(ConfigurationService.ApplicationConfiguration);
            return Json(true);
        }

        [HttpPost("[action]")]
        public IActionResult Save([FromBody]ApplicationConfigurationModel newModel)
        {
            var newConfig = Mapper.Map<ApplicationConfiguration>(newModel);
            ConfigurationService.SaveConfiguration(newConfig);
            return Json(Get());
        }
    }
}
