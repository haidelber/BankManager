using System.Collections.Generic;
using System.IO;
using System.Text;
using AutoMapper;
using BankManager.Core.Model.Configuration;
using BankManager.Core.Service;
using Microsoft.AspNetCore.Mvc;
using IConfigurationProvider = BankManager.Core.Provider.IConfigurationProvider;

namespace BankManager.Ui.Controllers
{
    public class ConfigurationController : ApiController
    {
        public IConfigurationProvider ConfigurationProvider { get; }
        public IConfigurationService ConfigurationService { get; }
        public IMapper Mapper { get; }

        public ConfigurationController(IConfigurationProvider configurationProvider, IMapper mapper, IConfigurationService configurationService)
        {
            ConfigurationProvider = configurationProvider;
            Mapper = mapper;
            ConfigurationService = configurationService;
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(string), 200)]
        public IActionResult ApplicationConfiguration()
        {
            return Json(ConfigurationProvider.ExportConfiguration());
        }

        [HttpGet("Path")]
        [ProducesResponseType(typeof(string), 200)]
        public IActionResult ConfigurationFilePath()
        {
            return Json(ConfigurationProvider.ConfigurationFilePath);
        }

        [HttpPost("Generate")]
        [ProducesResponseType(typeof(bool), 200)]
        public IActionResult GenerateConfigFile()
        {
            ConfigurationProvider.SaveConfiguration(ConfigurationProvider.ApplicationConfiguration);
            return Json(true);
        }

        [HttpGet("KeePass")]
        [ProducesResponseType(typeof(KeePassConfigurationModel), 200)]
        public IActionResult KeePassConfiguration()
        {
            return Json(ConfigurationService.KeePassConfiguration());
        }

        [HttpPost("KeePass")]
        [ProducesResponseType(typeof(KeePassConfigurationModel), 200)]
        public IActionResult KeePassConfiguration([FromBody]KeePassConfigurationModel model)
        {
            return Json(ConfigurationService.EditKeePassConfiguration(model));
        }

        [HttpGet("Database")]
        [ProducesResponseType(typeof(DatabaseConfigurationModel), 200)]
        public IActionResult DatabaseConfiguration()
        {
            return Json(ConfigurationService.DatabaseConfiguration());
        }

        [HttpPost("Database")]
        [ProducesResponseType(typeof(DatabaseConfigurationModel), 200)]
        public IActionResult DatabaseConfiguration([FromBody]DatabaseConfigurationModel model)
        {
            return Json(ConfigurationService.EditDatabaseConfiguration(model));
        }

        [HttpGet("DownloadHandler")]
        [ProducesResponseType(typeof(IEnumerable<DownloadHandlerConfigurationModel>), 200)]
        public IActionResult DownloadHandlerConfigurations()
        {
            return Json(ConfigurationService.DownloadHandlerConfigurations());
        }

        [HttpPost("DownloadHandler")]
        [ProducesResponseType(typeof(DownloadHandlerConfigurationModel), 200)]
        public IActionResult DownloadHandlerConfiguration([FromBody]DownloadHandlerConfigurationModel model)
        {
            return Json(ConfigurationService.EditDownloadHandlerConfiguration(model));
        }

        [HttpGet("DownloadHandler/{key}")]
        [ProducesResponseType(typeof(DownloadHandlerConfigurationModel), 200)]
        public IActionResult DownloadHandlerConfiguration([FromRoute]string key)
        {
            return Json(ConfigurationService.DownloadHandlerConfiguration(key));
        }
    }
}
