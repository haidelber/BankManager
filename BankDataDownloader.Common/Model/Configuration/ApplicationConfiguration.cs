using System.Collections.Generic;
using NLog;

namespace BankDataDownloader.Common.Model.Configuration
{
    public class ApplicationConfiguration
    {
        public string DownloadHandlerPath { get; set; }
        
        public IDictionary<string,DownloadHandlerConfiguration> DownloadHandlerConfigurations { get; set; }
        public KeePassConfiguration KeePassConfiguration { get; set; }
        public UiConfiguration UiConfiguration { get; set; }
    }
}
