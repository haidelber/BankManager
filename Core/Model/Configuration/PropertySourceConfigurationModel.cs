using System;
using System.Collections.Generic;
using BankManager.Common.Model.Configuration;

namespace BankManager.Core.Model.Configuration
{
    public class PropertySourceConfigurationModel
    {
        public Type TargetType { get; set; }
        public ValueParser Parser { get; set; } = ValueParser.String;
        public IDictionary<string, object> ValueParserParameter { get; set; }
    }
}