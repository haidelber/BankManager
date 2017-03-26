using System.Collections.Generic;

namespace BankDataDownloader.Core.Model.Configuration
{
    public class DownloadHandlerConfigurationModel
    {
        public string DownloadPath { get; set; }
        public string WebSiteUrl { get; set; }
        public string KeePassEntryUuid { get; set; }
        public IDictionary<string, string> AdditionalKeePassFields { get; set; }
    }
}
