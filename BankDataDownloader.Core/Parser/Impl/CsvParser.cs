using System;
using System.Collections.Generic;
using System.IO;
using Autofac;
using BankDataDownloader.Common.Model.Configuration;
using BankDataDownloader.Core.Extension;
using CsvHelper;
using CsvHelper.Configuration;

namespace BankDataDownloader.Core.Parser.Impl
{
    public class CsvParser : IFileParser
    {
        public FileParserConfiguration Configuration { get; }
        public IComponentContext Context { get; }

        public CsvParser(FileParserConfiguration configuration, IComponentContext context)
        {
            Configuration = configuration;
            Context = context;
        }

        private object GetValueForConfig(TableLikePropertySourceConfiguration config, CsvReader csv)
        {
            if (config == null)
                return null;
            string rawValue = null;
            if (Configuration.HasHeaderRow && config.ColumnName != null)
            {
                rawValue = csv[config.ColumnName];
            }
            if (rawValue != null) return rawValue;
            if (!config.ColumnIndex.HasValue)
            {
                throw new ArgumentException(
                    "No column index given although name couldn't be used as index or index is prefered",
                    "ColumnIndex");
            }
            rawValue = csv[config.ColumnIndex.Value];
            var parser = config.ResolveParser(Context);
            return parser.Parse(rawValue);
        }

        private object GetValueForConfig(FixedValuePropertySourceConfiguration config)
        {
            var parser = config?.ResolveParser(Context);
            return parser?.Parse(config.FixedValue);
        }

        public IEnumerable<object> Parse(string filePath)
        {
            using (var file = File.OpenRead(filePath))
            {
                using (var text = new StreamReader(file, Configuration.Encoding))
                {
                    var csvConf = new CsvConfiguration
                    {
                        HasHeaderRecord = Configuration.HasHeaderRow,
                        Delimiter = Configuration.Delimiter,
                        Quote = Configuration.Quote
                    };
                    using (var csv = new CsvReader(text, csvConf))
                    {
                        for (var i = 0; i < Configuration.SkipRows; i++)
                        {
                            csv.Read();
                        }
                        if (Configuration.HasHeaderRow)
                        {
                            csv.ReadHeader();
                        }
                        while (csv.Read())
                        {
                            var target = Activator.CreateInstance(Configuration.TargetType);
                            foreach (var conf in Configuration.PropertySourceConfiguration)
                            {
                                var tableLikeConfig = conf.Value as TableLikePropertySourceConfiguration;
                                var fixedConfig = conf.Value as FixedValuePropertySourceConfiguration;
                                if (tableLikeConfig == null && fixedConfig == null)
                                {
                                    throw new ArgumentException(
                                             "PropertySourceConfiguration is no TableLikePropertySourceConfiguration or FixedValuePropertySourceConfiguration",
                                             conf.Key);
                                }


                                var value = GetValueForConfig(tableLikeConfig, csv) ??
                                               GetValueForConfig(fixedConfig);

                                var prop = Configuration.TargetType.GetProperty(conf.Key);
                                prop.SetValue(target, value);
                            }
                            yield return target;
                        }
                    }
                }
            }
        }
    }
}

