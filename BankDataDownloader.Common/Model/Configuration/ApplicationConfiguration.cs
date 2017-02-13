using System;
using System.Collections.Generic;
using NLog;

namespace BankDataDownloader.Common.Model.Configuration
{
    public class ApplicationConfiguration
    {
        public string DownloadHandlerPath { get; set; }

        /// <summary>
        /// all download handler configurations for a given key (e.g. Raiffeisen, DKB)
        /// </summary>
        public IDictionary<string, DownloadHandlerConfiguration> DownloadHandlerConfigurations { get; set; }
        /// <summary>
        /// all parser configurations for a given key (e.g. Raiffeisen, DKB)
        /// </summary>
        public IDictionary<string, FileParserConfiguration> FileParserConfiguration { get; set; }
        public KeePassConfiguration KeePassConfiguration { get; set; }
        public DatabaseConfiguration DatabaseConfiguration { get; set; }
        public UiConfiguration UiConfiguration { get; set; }

        public ApplicationConfiguration()
        {
            DownloadHandlerConfigurations = new Dictionary<string, DownloadHandlerConfiguration>();
            FileParserConfiguration = new Dictionary<string, FileParserConfiguration>();
        }
    }
}
