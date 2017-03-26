using System;
using System.Collections.Generic;
using Autofac;
using AutoMapper;
using BankDataDownloader.Common.Extensions;
using BankDataDownloader.Common.Model.Configuration;
using BankDataDownloader.Core.DownloadHandler;
using BankDataDownloader.Core.Model.DownloadHandler;
using BankDataDownloader.Core.Service;
using BankDataDownloader.Core.ValueProvider;
using Microsoft.AspNetCore.Mvc;

namespace BankManager.Ui.Controllers
{

    public class DownloadController : ApiController
    {

        public IKeePassPasswordValueProvider KeePassPasswordValueProvider { get; }
        public IKeePassService KeePassService { get; }
        public IComponentContext Container { get; }
        public IMapper Mapper { get; }
        public ApplicationConfiguration ApplicationConfiguration { get; }

        public DownloadController(IKeePassPasswordValueProvider keePassPasswordValueProvider, IComponentContext container, IMapper mapper, ApplicationConfiguration applicationConfiguration, IKeePassService keePassService)
        {
            KeePassPasswordValueProvider = keePassPasswordValueProvider;
            Container = container;
            Mapper = mapper;
            ApplicationConfiguration = applicationConfiguration;
            KeePassService = keePassService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Json(GetDownloadHandlerOverview());
        }

        [HttpPost("[action]")]
        public IActionResult Run([FromBody]DownloadHandlerRunModel runModel)
        {
            RunDownloadHandler(runModel);
            //TODO add signalR for posting back status updates
            return Json(Guid.NewGuid());
        }

        private IEnumerable<DownloadHandlerOverviewModel> GetDownloadHandlerOverview()
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

        private void RunDownloadHandler(DownloadHandlerRunModel runModel)
        {
            KeePassPasswordValueProvider.RegisterPassword(runModel.KeePassPassword.ConvertToSecureString());
            if (KeePassService.CheckPassword())
            {
                foreach (var handlerKey in runModel.DownloadHandlerKeys)
                {
                    var downloadHandler = Container.ResolveKeyed<IBankDownloadHandler>(handlerKey);

                    try
                    {
                        downloadHandler.Execute(true);
                    }
                    catch (InvalidOperationException ex)
                    {
                        //this is just a failed check balance 
                        Logger.Info(ex, $"Failed balance check for {handlerKey}");
                    }
                }
            }
            else
            {
                //TODO message
            }
            KeePassPasswordValueProvider.DeregisterPassword();
        }
    }
}
