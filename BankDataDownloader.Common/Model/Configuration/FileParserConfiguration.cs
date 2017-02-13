using System.Collections.Generic;
using Newtonsoft.Json;

namespace BankDataDownloader.Common.Model.Configuration
{
    public class FileParserConfiguration
    {
        [JsonIgnore]
        public ApplicationConfiguration ApplicationConfiguration { get; set; }

        public IDictionary<string, PropertySourceConfiguration> PropertySourceConfiguration { get; set; }
        public bool HasHeaderRow { get; set; } = true;
        public int SkipRows { get; set; } = 0;

        public FileParserConfiguration()
        {
            PropertySourceConfiguration = new Dictionary<string, PropertySourceConfiguration>();
        }
    }

    public class PropertySourceConfiguration
    {
        public string ValueType { get; set; } = typeof (string).ToString();
        public ValueParser Parser { get; set; }=ValueParser.String;
        public IDictionary<string, object> ValueParserParameter { get; set; }

        public PropertySourceConfiguration()
        {
            ValueParserParameter = new Dictionary<string, object>();
        }
    }

    public enum ValueParser
    {
        String,GermanDecimal, EnglishDecimal, DateTime, Enum
    }

    public class TableLikePropertySourceConfiguration : PropertySourceConfiguration
    {
        public string ColumnName { get; set; }
        public int ColumnIndex { get; set; }
        public bool PreferName { get; set; } = true;
    }
}