﻿using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace BankDataDownloader.Common.Model.Configuration
{
    public class DownloadHandlerConfiguration
    {
        [JsonIgnore]
        public ApplicationConfiguration ApplicationConfiguration { get; set; }

        public string WebSiteUrl { get; set; }
        public string KeePassEntryUuid { get; set; }
        public string RelativeDownloadPath { get; set; }
        public IDictionary<string,string> AdditionalKeePassFields { get; set; }

        [JsonIgnore]
        public string DownloadPath => Path.Combine(ApplicationConfiguration.DownloadHandlerPath, RelativeDownloadPath);
    }
}
