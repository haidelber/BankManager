using System.Collections.Generic;

namespace BankDataDownloader.Common.Model.Configuration
{
    public class ApplicationConfiguration
    {
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

        protected bool Equals(ApplicationConfiguration other)
        {
            return Equals(DownloadHandlerConfigurations, other.DownloadHandlerConfigurations) && Equals(FileParserConfiguration, other.FileParserConfiguration) && Equals(KeePassConfiguration, other.KeePassConfiguration) && Equals(DatabaseConfiguration, other.DatabaseConfiguration) && Equals(UiConfiguration, other.UiConfiguration);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ApplicationConfiguration) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (DownloadHandlerConfigurations != null ? DownloadHandlerConfigurations.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (FileParserConfiguration != null ? FileParserConfiguration.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (KeePassConfiguration != null ? KeePassConfiguration.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (DatabaseConfiguration != null ? DatabaseConfiguration.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (UiConfiguration != null ? UiConfiguration.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
