using System;
using System.Collections.Generic;
using BankDataDownloader.Common.Converter;
using BankDataDownloader.Common.Extensions;
using Newtonsoft.Json;

namespace BankDataDownloader.Common.Model.Configuration
{
    public class PropertySourceConfiguration
    {
         public Type TargetType { get; set; }
        public ValueParser Parser { get; set; } = ValueParser.String;
        public IDictionary<string, object> ValueParserParameter { get; set; }

        public PropertySourceConfiguration()
        {
            ValueParserParameter = new Dictionary<string, object>();
        }

        protected bool Equals(PropertySourceConfiguration other)
        {
            return Equals(TargetType, other.TargetType) && Parser == other.Parser && ValueParserParameter.DictionaryEqual( other.ValueParserParameter);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PropertySourceConfiguration) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (TargetType != null ? TargetType.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (int) Parser;
                hashCode = (hashCode*397) ^ (ValueParserParameter != null ? ValueParserParameter.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}