using System;
using System.Collections.Generic;
using System.Text;
using BankDataDownloader.Common.Converter;
using Newtonsoft.Json;

namespace BankDataDownloader.Common.Model.Configuration
{
    public class FileParserConfiguration
    {
        [JsonConverter(typeof(TypeConverter))]
        public Type TargetType { get; set; }
        public IDictionary<string, object> PropertySourceConfiguration { get; set; }
        public bool HasHeaderRow { get; set; } = true;
        public int SkipRows { get; set; } = 0;
        //TODO custom converter
        [JsonIgnore]
        public Encoding Encoding { get; set; } = Encoding.Default;
        public string Delimiter { get; set; } = ";";
        public char Quote { get; set; } = '\"';

        public FileParserConfiguration()
        {
            PropertySourceConfiguration = new Dictionary<string, object>();
        }
    }
}