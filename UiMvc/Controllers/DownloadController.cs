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
    public class DownloadController : Controller
    {
        public IKeePassPasswordValueProvider KeePassPasswordValueProvider { get; }
        public KeePassConfiguration KeePassConfiguration { get; }
        public DatabaseConfiguration DatabaseConfiguration { get; }
        public IComponentContext Container { get; }
        public IMapper Mapper { get; }
        public ApplicationConfiguration ApplicationConfiguration { get; }

        public DownloadController(IKeePassPasswordValueProvider keePassPasswordValueProvider, KeePassConfiguration keePassConfiguration, DatabaseConfiguration databaseConfiguration, IComponentContext container, IMapper mapper, ApplicationConfiguration applicationConfiguration)
        {
            KeePassPasswordValueProvider = keePassPasswordValueProvider;
            KeePassConfiguration = keePassConfiguration;
            DatabaseConfiguration = databaseConfiguration;
            Container = container;
            Mapper = mapper;
            ApplicationConfiguration = applicationConfiguration;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View(DownloadHandlerOverview());
        }

        private IEnumerable<DownloadHandlerOverviewModel> DownloadHandlerOverview()
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

        [HttpPost]
        public Guid RunDownloadHandler(DownloadHandlerRunModel runModel)
        {
            KeePassPasswordValueProvider.RegisterPassword(runModel.KeePassPassword.ConvertToSecureString());
            Parallel.ForEach(runModel.DownloadHandlerKeys, (handlerKey) =>
            {
                var downloadHandler = Container.ResolveKeyed<IBankDownloadHandler>(handlerKey);

                try
                {
                    downloadHandler.Execute(true);
                }
                catch (InvalidOperationException ex)
                {
                    //this is just a failed check balance 
                }
            });
            KeePassPasswordValueProvider.DeregisterPassword();
            //TODO add signalR for posting back status updates
            return Guid.NewGuid();
        }
    }
}
