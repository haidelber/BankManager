using System;
using Newtonsoft.Json;

namespace BankDataDownloader.Common.Converter
{
    public class TypeConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var type = (Type)value;
            var typeSer = new TypeSerialization
            {
                AssemblyQualifiedName = type.AssemblyQualifiedName
            };
            serializer.Serialize(writer, typeSer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var typeSer = serializer.Deserialize<TypeSerialization>(reader);
            return Type.GetType(typeSer.AssemblyQualifiedName);
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(Type) == objectType;
        }

        public class TypeSerialization
        {
            public string AssemblyQualifiedName { get; set; }
        }
    }
}
