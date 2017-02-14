using System.IO;
using System.Linq;
using System.Text;
using Autofac;
using BankDataDownloader.Common;
using BankDataDownloader.Common.Model.Configuration;
using BankDataDownloader.Core.DownloadHandler;
using BankDataDownloader.Core.DownloadHandler.Impl;
using BankDataDownloader.Core.ValueProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace DataDownloader.Test.DownloadHandler
{
    [TestClass]
    public class RaiffeisenDownloadHandlerTest : ContainerBasedTestBase
    {
        public DownloadHandlerConfiguration DownloadHandlerConfiguration { get; set; }
        public RaiffeisenDownloadHandler RaiffeisenDownloadHandler { get; set; }
        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();

            RaiffeisenDownloadHandler = Container.Resolve<RaiffeisenDownloadHandler>();
            DownloadHandlerConfiguration = Container.ResolveNamed<DownloadHandlerConfiguration>(Constants.UniqueContainerKeys.DownloadHandlerRaiffeisen);

            DownloadHandlerConfiguration.DownloadPath = TestConstants.DownloadHandler.RaiffeisenPath;
            DownloadHandlerConfiguration.KeePassEntryUuid = TestConstants.Service.KeePass.RaiffeisenUuid;
        }
        [TestMethod]
        public void Test()
        {
            RaiffeisenDownloadHandler.Execute(true);
        }
    }

}
