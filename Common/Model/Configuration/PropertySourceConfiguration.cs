using System;
using System.Collections.Generic;
using BankDataDownloader.Common.Converter;
using Newtonsoft.Json;

namespace BankDataDownloader.Common.Model.Configuration
{
    public class PropertySourceConfiguration
    {
        [JsonConverter(typeof(TypeConverter))]
        public Type TargetType { get; set; }
        public ValueParser Parser { get; set; } = ValueParser.String;
        public IDictionary<string, object> ValueParserParameter { get; set; }

        public PropertySourceConfiguration()
        {
            ValueParserParameter = new Dictionary<string, object>();
        }
    }
}