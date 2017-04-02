using System;
using System.Collections.Generic;
using BankDataDownloader.Common.Model.Configuration;

namespace BankDataDownloader.Core.Model.Configuration
{
    public class PropertySourceConfigurationModel
    {
        public Type TargetType { get; set; }
        public ValueParser Parser { get; set; } = ValueParser.String;
        public IDictionary<string, object> ValueParserParameter { get; set; }
    }
}