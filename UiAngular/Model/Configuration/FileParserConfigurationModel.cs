using System;
using System.Collections.Generic;
using System.Text;
using BankDataDownloader.Common.Converter;
using BankDataDownloader.Common.Extensions;
using Newtonsoft.Json;

namespace UiAngular.Model.Configuration
{
    public class FileParserConfigurationModel
    {
        public Type ParserType { get; set; }
        public Type TargetType { get; set; }
        public IDictionary<string, PropertySourceConfigurationModel> PropertySourceConfiguration { get; set; }
        public bool HasHeaderRow { get; set; } = true;
        public int SkipRows { get; set; } = 0;
        public Encoding Encoding { get; set; } = Encoding.Default;
        public string Delimiter { get; set; } = ";";
        public char Quote { get; set; } = '\"';
    }
}