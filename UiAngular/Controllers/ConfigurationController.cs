using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BankDataDownloader.Common.Model.Configuration;
using BankDataDownloader.Core.Service;
using Microsoft.AspNetCore.Mvc;
using UiAngular.Model.Configuration;

namespace UiAngular.Controllers
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
