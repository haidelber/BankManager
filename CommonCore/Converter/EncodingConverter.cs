using System;
using System.Text;
using Newtonsoft.Json;

namespace BankDataDownloader.Common.Converter
{
    public class EncodingConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var encoding = (Encoding)value;
            var ser = new EncodingSerialization
            {
                CodePage = encoding.CodePage,
                Name = encoding.BodyName
            };
            serializer.Serialize(writer, ser);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var encodingSerialization = serializer.Deserialize<EncodingSerialization>(reader);
            return Encoding.GetEncoding(encodingSerialization.CodePage);
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(Encoding) == objectType;
        }

        public class EncodingSerialization
        {
            public int CodePage { get; set; }
            public string Name { get; set; }
        }
    }
}