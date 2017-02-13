using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace BankDataDownloader.Common.Model.Configuration
{
    public class FileParserConfiguration
    {
        [JsonIgnore]
        public ApplicationConfiguration ApplicationConfiguration { get; set; }

        public IDictionary<string, object> PropertySourceConfiguration { get; set; }
        public bool HasHeaderRow { get; set; } = true;
        public int SkipRows { get; set; } = 0;

        public Encoding Encoding { get; set; } = Encoding.Default;
        public string Delimiter { get; set; } = ";";
        public char Quote { get; set; } = '\"';

        public FileParserConfiguration()
        {
            PropertySourceConfiguration = new Dictionary<string, object>();
        }
    }
}