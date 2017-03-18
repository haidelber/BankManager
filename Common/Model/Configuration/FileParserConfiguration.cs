using System;
using System.Collections.Generic;
using System.Text;
using BankDataDownloader.Common.Converter;
using BankDataDownloader.Common.Extensions;
using Newtonsoft.Json;

namespace BankDataDownloader.Common.Model.Configuration
{
    public class FileParserConfiguration
    {
        public Type ParserType { get; set; }
        public Type TargetType { get; set; }
        public IDictionary<string, PropertySourceConfiguration> PropertySourceConfiguration { get; set; }
        public bool HasHeaderRow { get; set; } = true;
        public int SkipRows { get; set; } = 0;
        public Encoding Encoding { get; set; } = Encoding.Default;
        public string Delimiter { get; set; } = ";";
        public char Quote { get; set; } = '\"';

        public ExcelVersion ExcelVersion { get; set; }
        public string TableName { get; set; }
        public int? TableIndex { get; set; }

        public FileParserConfiguration()
        {
            PropertySourceConfiguration = new Dictionary<string, PropertySourceConfiguration>();
        }

        protected bool Equals(FileParserConfiguration other)
        {
            return Equals(ParserType, other.ParserType) && Equals(TargetType, other.TargetType) && HasHeaderRow == other.HasHeaderRow && SkipRows == other.SkipRows && Equals(Encoding, other.Encoding) && string.Equals(Delimiter, other.Delimiter) && Quote == other.Quote && ExcelVersion == other.ExcelVersion && string.Equals(TableName, other.TableName) && TableIndex == other.TableIndex && PropertySourceConfiguration.DictionaryEqual(other.PropertySourceConfiguration);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((FileParserConfiguration)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (ParserType != null ? ParserType.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (TargetType != null ? TargetType.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ HasHeaderRow.GetHashCode();
                hashCode = (hashCode * 397) ^ SkipRows;
                hashCode = (hashCode * 397) ^ (Encoding != null ? Encoding.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Delimiter != null ? Delimiter.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Quote.GetHashCode();
                hashCode = (hashCode * 397) ^ (int)ExcelVersion;
                hashCode = (hashCode * 397) ^ (TableName != null ? TableName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ TableIndex.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(FileParserConfiguration left, FileParserConfiguration right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(FileParserConfiguration left, FileParserConfiguration right)
        {
            return !Equals(left, right);
        }
    }
}