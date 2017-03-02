using System.Collections.Generic;
using System.Linq;
using BankDataDownloader.Common.Extensions;
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

        protected bool Equals(DownloadHandlerConfiguration other)
        {
            var keep = AdditionalKeePassFields.DictionaryEqual(other.AdditionalKeePassFields);
            return string.Equals(DownloadPath, other.DownloadPath) && string.Equals(WebSiteUrl, other.WebSiteUrl) && string.Equals(KeePassEntryUuid, other.KeePassEntryUuid) && keep;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DownloadHandlerConfiguration)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (DownloadPath != null ? DownloadPath.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (WebSiteUrl != null ? WebSiteUrl.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (KeePassEntryUuid != null ? KeePassEntryUuid.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (AdditionalKeePassFields != null ? AdditionalKeePassFields.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
