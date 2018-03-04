using System;
using System.Collections.Generic;
using BankManager.Common.Extensions;
using Newtonsoft.Json;

namespace BankManager.Common.Model.Configuration
{
    public class DownloadHandlerConfiguration
    {
        [JsonIgnore]
        public ApplicationConfiguration ApplicationConfiguration { get; set; }

        public Type DownloadHandlerType { get; set; }
        public string DownloadPath { get; set; }
        public string WebSiteUrl { get; set; }
        public string KeePassEntryUuid { get; set; }
        public string DisplayName { get; set; }
        public bool DefaultSelected { get; set; }

        public IDictionary<string, string> AdditionalKeePassFields { get; set; }

        public DownloadHandlerConfiguration()
        {
            AdditionalKeePassFields = new Dictionary<string, string>();
            DefaultSelected = true;
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
