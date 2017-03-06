using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using AutoMapper;
using BankDataDownloader.Common;
using BankDataDownloader.Common.Extensions;
using BankDataDownloader.Common.Model.Configuration;
using BankDataDownloader.Core.DownloadHandler;
using BankDataDownloader.Core.DownloadHandler.Impl;
using BankDataDownloader.Core.ValueProvider;
using Microsoft.AspNetCore.Mvc;
using UiAngular.Model;
using UiAngular.Model.DownloadHandler;

namespace UiAngular.Controllers
{
    [Route("api/[controller]")]
    public class DownloadHandlerController : Controller
    {
        public IKeePassPasswordValueProvider KeePassPasswordValueProvider { get; }
        public KeePassConfiguration KeePassConfiguration { get; }
        public DatabaseConfiguration DatabaseConfiguration { get; }
        public IComponentContext Container { get; }
        public IMapper Mapper { get; }
        public ApplicationConfiguration ApplicationConfiguration { get; }

        public DownloadHandlerController(IKeePassPasswordValueProvider keePassPasswordValueProvider, KeePassConfiguration keePassConfiguration, DatabaseConfiguration databaseConfiguration, IComponentContext container, IMapper mapper, ApplicationConfiguration applicationConfiguration)
        {
            KeePassPasswordValueProvider = keePassPasswordValueProvider;
            KeePassConfiguration = keePassConfiguration;
            DatabaseConfiguration = databaseConfiguration;
            Container = container;
            Mapper = mapper;
            ApplicationConfiguration = applicationConfiguration;
        }

        [HttpGet]
        public IEnumerable<DownloadHandlerOverviewModel> Get()
        {
            foreach (var config in ApplicationConfiguration.DownloadHandlerConfigurations)
            {

                yield return new DownloadHandlerOverviewModel
                {
                    Name = config.Key,
                    Path = config.Value.DownloadPath,
                    Url = config.Value.WebSiteUrl,
                    DefaultSelected = true
                };
            }
        }

        [HttpPost("[action]")]
        public void RunDownloadHandler(DownloadHandlerRunModel runModel)
        {
            KeePassPasswordValueProvider.RegisterPassword(runModel.KeePassPassword.ConvertToSecureString());
            Parallel.ForEach(runModel.DownloadHandlerKeys, (handlerKey) =>
            {
                var downloadHandler = Container.ResolveKeyed<IBankDownloadHandler>(handlerKey);

                downloadHandler.Execute(true);
            });
        }
    }
}
