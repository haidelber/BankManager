using System;
using Newtonsoft.Json;

namespace BankDataDownloader.Common.Converter
{
    public class TypeConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var type = (Type)value;
            serializer.Serialize(writer, type.AssemblyQualifiedName);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var typeSer = serializer.Deserialize<string>(reader);
            return Type.GetType(typeSer);
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(Type) == objectType;
        }
    }
}
