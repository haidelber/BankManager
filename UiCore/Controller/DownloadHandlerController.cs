using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Autofac;
using BankDataDownloader.Common;
using BankDataDownloader.Common.Model.Configuration;
using BankDataDownloader.Core.DownloadHandler.Impl;
using BankDataDownloader.Core.ValueProvider;
using BankDataDownloader.Core.ValueProvider.Impl;

namespace UiCore.Controller
{
    public class DownloadHandlerController : ApiController
    {
        public DkbDownloadHandler DkbDownloadHandler { get; }
        public IKeePassPasswordValueProvider KeePassPasswordValueProvider { get; }
        public KeePassConfiguration KeePassConfiguration { get; }
        public DatabaseConfiguration DatabaseConfiguration { get; }
        public IComponentContext Container { get;  }

        public DownloadHandlerController(DkbDownloadHandler dkbDownloadHandler, IKeePassPasswordValueProvider keePassPasswordValueProvider, KeePassConfiguration keePassConfiguration, DatabaseConfiguration databaseConfiguration, IComponentContext container)
        {
            DkbDownloadHandler = dkbDownloadHandler;
            KeePassPasswordValueProvider = keePassPasswordValueProvider;
            KeePassConfiguration = keePassConfiguration;
            DatabaseConfiguration = databaseConfiguration;
            Container = container;
        }

        [HttpGet]
        public void RunDkb()
        {
            KeePassConfiguration.Path = TestConstants.Service.KeePass.Path;
            KeePassPasswordValueProvider.RegisterPassword(TestConstants.Service.KeePass.Password);
            DatabaseConfiguration.DatabasePath = TestConstants.Data.DatabasePath;
            var downloadHandlerConfiguration =
                Container.ResolveKeyed<DownloadHandlerConfiguration>(
                    Constants.UniqueContainerKeys.DownloadHandlerDkb);
            downloadHandlerConfiguration.DownloadPath = TestConstants.DownloadHandler.DkbPath;
            downloadHandlerConfiguration.KeePassEntryUuid = TestConstants.Service.KeePass.DkbUuid;
            DkbDownloadHandler.Execute(true);
        }
    }
}
