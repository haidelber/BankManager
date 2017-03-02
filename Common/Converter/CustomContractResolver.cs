using System;
using System.Text;
using BankDataDownloader.Common.Model.Configuration;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace BankDataDownloader.Common.Converter
{
    public class CustomContractResolver : DefaultContractResolver, IContractResolver
    {
        public EncodingConverter EncodingConverter { get; }
        public TypeConverter TypeConverter { get; }

        public CustomContractResolver(TypeConverter typeConverter, EncodingConverter encodingConverter)
        {
            TypeConverter = typeConverter;
            EncodingConverter = encodingConverter;
        }

        protected override JsonContract CreateContract(Type objectType)
        {
            JsonContract contract = base.CreateContract(objectType);

            // this will only be called once and then cached
            if (typeof(Encoding).IsAssignableFrom(objectType))
            {
                contract.Converter = EncodingConverter;
            }
            else if (typeof(Type).IsAssignableFrom(objectType))
            {
                contract.Converter = TypeConverter;
            }
            else
            {
                return base.CreateContract(objectType);
            }
            return contract;
        }
    }
}