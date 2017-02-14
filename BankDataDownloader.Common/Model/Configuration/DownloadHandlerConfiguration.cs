using System.Collections.Generic;
using Newtonsoft.Json;

namespace BankDataDownloader.Common.Model.Configuration
{
    public class DownloadHandlerConfiguration
    {
        [JsonIgnore]
        public ApplicationConfiguration ApplicationConfiguration { get; set; }

        public string DownloadPath { get; set; }
        public string WebSiteUrl { get; set; }
        public string KeePassEntryUuid { get; set; }
        public IDictionary<string, string> AdditionalKeePassFields { get; set; }

        public DownloadHandlerConfiguration()
        {
            AdditionalKeePassFields = new Dictionary<string, string>();
        }
    }
}
