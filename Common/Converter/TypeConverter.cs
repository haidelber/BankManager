﻿using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Text;
using Newtonsoft.Json;

namespace BankDataDownloader.Common.Converter
{
    public class TypeConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var type = (Type)value;
            serializer.Serialize(writer, GetTypeName(type, FormatterAssemblyStyle.Simple, serializer.Binder));
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
        /// <summary>
        /// Taken from Newtonsoft.Json
        /// </summary>
        /// <param name="t"></param>
        /// <param name="assemblyFormat"></param>
        /// <param name="binder"></param>
        /// <returns></returns>
        public static string GetTypeName(Type t, FormatterAssemblyStyle assemblyFormat, SerializationBinder binder)
        {
            string fullyQualifiedTypeName;
            if (binder != null)
            {
                string assemblyName;
                string typeName;
                binder.BindToName(t, out assemblyName, out typeName);
                fullyQualifiedTypeName = typeName + (assemblyName == null ? "" : ", " + assemblyName);
            }
            else
                fullyQualifiedTypeName = t.AssemblyQualifiedName;
            switch (assemblyFormat)
            {
                case FormatterAssemblyStyle.Simple:
                    return RemoveAssemblyDetails(fullyQualifiedTypeName);
                case FormatterAssemblyStyle.Full:
                    return fullyQualifiedTypeName;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        private static string RemoveAssemblyDetails(string fullyQualifiedTypeName)
        {
            StringBuilder stringBuilder = new StringBuilder();
            bool flag1 = false;
            bool flag2 = false;
            for (int index = 0; index < fullyQualifiedTypeName.Length; ++index)
            {
                char ch = fullyQualifiedTypeName[index];
                switch (ch)
                {
                    case ',':
                        if (!flag1)
                        {
                            flag1 = true;
                            stringBuilder.Append(ch);
                            break;
                        }
                        flag2 = true;
                        break;
                    case '[':
                        flag1 = false;
                        flag2 = false;
                        stringBuilder.Append(ch);
                        break;
                    case ']':
                        flag1 = false;
                        flag2 = false;
                        stringBuilder.Append(ch);
                        break;
                    default:
                        if (!flag2)
                        {
                            stringBuilder.Append(ch);
                            break;
                        }
                        break;
                }
            }
            return stringBuilder.ToString();
        }
    }
}