using System;
using System.Collections.Generic;
using Autofac;
using AutoMapper;
using BankManager.Common.Exceptions;
using BankManager.Common.Extensions;
using BankManager.Common.Model.Configuration;
using BankManager.Core.DownloadHandler;
using BankManager.Core.Model.DownloadHandler;
using BankManager.Core.Provider;
using BankManager.Core.Service;
using Microsoft.AspNetCore.Mvc;

namespace BankManager.Ui.Controllers
{

    public class DownloadController : ApiController
    {

        public IKeePassPasswordProvider KeePassPasswordProvider { get; }
        public IKeePassService KeePassService { get; }
        public IComponentContext Container { get; }
        public IMapper Mapper { get; }
        public ApplicationConfiguration ApplicationConfiguration { get; }

        public DownloadController(IKeePassPasswordProvider keePassPasswordProvider, IComponentContext container, IMapper mapper, ApplicationConfiguration applicationConfiguration, IKeePassService keePassService)
        {
            KeePassPasswordProvider = keePassPasswordProvider;
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
            KeePassPasswordProvider.RegisterPassword(runModel.KeePassPassword.ConvertToSecureString());
            if (KeePassService.CheckPassword())
            {
                foreach (var handlerKey in runModel.DownloadHandlerKeys)
                {
                    try
                    {
                        using (var downloadHandler = Container.ResolveKeyed<IBankDownloadHandler>(handlerKey))
                        {
                            downloadHandler.Execute(true);
                        }
                    }
                    catch (BalanceCheckException ex)
                    {
                        //this is just a failed check balance 
                        Logger.Warn(ex,
                            $"Failed balance check for {handlerKey} expected {ex.Expected} actual {ex.Actual}. Be aware of pending transactions which might influence the expected balance. Especially when importing during nighttime.");
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex, $"Unexpected exception occured for {handlerKey}");
                    }
                }
            }
            else
            {
                //TODO message
            }
            KeePassPasswordProvider.DeregisterPassword();
        }
    }
}
