using System;
using System.Collections.Generic;
using BankDataDownloader.Common.Converter;
using BankDataDownloader.Common.Extensions;
using BankDataDownloader.Common.Model.Configuration;
using Newtonsoft.Json;

namespace UiAngular.Model.Configuration
{
    public class PropertySourceConfigurationModel
    {
        public Type TargetType { get; set; }
        public ValueParser Parser { get; set; } = ValueParser.String;
        public IDictionary<string, object> ValueParserParameter { get; set; }
    }
}