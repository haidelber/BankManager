using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using BankDataDownloader.Common.Extensions;
using BankDataDownloader.Common.Model.Configuration;
using BankDataDownloader.Core.DownloadHandler;
using BankDataDownloader.Core.ValueProvider;
using BankDataDownloader.Ui.Model.DownloadHandler;
using Microsoft.AspNetCore.Mvc;

namespace BankDataDownloader.Ui.Controllers
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
                    Key = config.Key,
                    Path = config.Value.DownloadPath,
                    Url = config.Value.WebSiteUrl,
                    DisplayName = config.Value.DisplayName,
                    Selected = true
                };
            }
        }

        [HttpPost("[action]")]
        public Guid RunDownloadHandler([FromBody]DownloadHandlerRunModel runModel)
        {
            KeePassPasswordValueProvider.RegisterPassword(runModel.KeePassPassword.ConvertToSecureString());
            Parallel.ForEach(runModel.DownloadHandlerKeys, (handlerKey) =>
            {
                var downloadHandler = Container.ResolveKeyed<IBankDownloadHandler>(handlerKey);

                downloadHandler.Execute(true);
            });
            return Guid.NewGuid();
        }
    }
}
