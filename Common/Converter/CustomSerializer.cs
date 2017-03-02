using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog.Targets;

namespace BankDataDownloader.Common.Converter
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
