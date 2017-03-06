using System.Collections.Generic;
using System.Linq;
using BankDataDownloader.Common.Extensions;
using Newtonsoft.Json;

namespace UiAngular.Model.Configuration
{
    public class DownloadHandlerConfigurationModel
    {
        public string DownloadPath { get; set; }
        public string WebSiteUrl { get; set; }
        public string KeePassEntryUuid { get; set; }
        public IDictionary<string, string> AdditionalKeePassFields { get; set; }
    }
}
