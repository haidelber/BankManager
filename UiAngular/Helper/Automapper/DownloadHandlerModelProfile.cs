using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Core;
using AutoMapper;
using BankDataDownloader.Common.Model.Configuration;
using UiAngular.Model;
using UiAngular.Model.Configuration;

namespace UiAngular.Helper.Automapper
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
