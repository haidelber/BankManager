﻿using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BankManager.Common.Converter
{
    public class CustomSerializer : JsonSerializer
    {
        public CustomSerializer(IContractResolver contractResolver)
        {
            ContractResolver = contractResolver;
            TypeNameHandling = TypeNameHandling.Auto;
        }
    }
}
