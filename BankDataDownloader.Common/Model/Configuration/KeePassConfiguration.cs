using System.Security;
using Newtonsoft.Json;

namespace BankDataDownloader.Common.Model.Configuration
{
    public class KeePassConfiguration
    {
        [JsonIgnore]
        public ApplicationConfiguration ApplicationConfiguration { get; set; }

        public string Path { get; set; }

        [JsonIgnore]
        public SecureString Password { get; set; }
        //TODO add support for composite database keys
        //TODO provide password not via IOC container
    }
}