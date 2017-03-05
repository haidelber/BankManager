using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

        private object GetValueForConfig(ColumnPropertySourceConfiguration config, CsvReader csv)
        {
            if (config == null)
                return null;
            string rawValue = null;
            if (Configuration.HasHeaderRow && config.ColumnName != null)
            {
                rawValue = csv[config.ColumnName];
            }
            if (rawValue == null && !config.ColumnIndex.HasValue)
            {
                throw new ArgumentException(
                    "No column index given although name couldn't be used as index or index is prefered",
                    "ColumnIndex");
            }
            Debug.Assert(config.ColumnIndex != null, "config.ColumnIndex != null");
            rawValue = csv[config.ColumnIndex.Value];
            var parser = config.ResolveParser(Context);
            return parser.Parse(rawValue);
        }

        private object GetValueForConfig(MultiColumnPropertySourceConfiguration config, CsvReader csv)
        {
            if (config == null)
                return null;
            string rawValue = null;
            var list = new List<string>();
            if (config.ColumnNames != null)
            {
                for (var index = 0; index < config.ColumnNames.Length; index++)
                {
                    var configColumnName = config.ColumnNames[index];
                    rawValue = csv[configColumnName];
                    list.Insert(index, rawValue ?? "");
                }
            }
            if (config.ColumnIndices != null)
            {
                for (var index = 0; index < config.ColumnIndices.Length; index++)
                {
                    if (list[index] == null)
                    {
                        var columnIndex = config.ColumnIndices[index];
                        if (columnIndex.HasValue)
                        {
                            rawValue = csv[columnIndex.GetValueOrDefault()];
                            list[index] = rawValue;
                        }
                        else
                        {
                            throw new ArgumentException(
                                "No column index given although name couldn't be used as index or index is prefered",
                                "ColumnIndex");
                        }
                    }
                }
            }
            rawValue = string.Format(config.FormatString, list.Cast<object>().ToArray());
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
                        Quote = Configuration.Quote,
                        TrimHeaders = true
                    };
                    for (var i = 0; i < Configuration.SkipRows; i++)
                    {
                        text.ReadLine();
                    }
                    using (var csv = new CsvReader(text, csvConf))
                    {
                        while (csv.Read())
                        {
                            var target = Activator.CreateInstance(Configuration.TargetType);
                            foreach (var conf in Configuration.PropertySourceConfiguration)
                            {
                                var columnPropertySourceConfiguration = conf.Value as ColumnPropertySourceConfiguration;
                                var fixedValuePropertySourceConfiguration = conf.Value as FixedValuePropertySourceConfiguration;
                                var multiColumnPropertySourceConfiguration = conf.Value as MultiColumnPropertySourceConfiguration;
                                if (columnPropertySourceConfiguration == null && multiColumnPropertySourceConfiguration == null && fixedValuePropertySourceConfiguration == null)
                                {
                                    throw new ArgumentException(
                                             "PropertySourceConfiguration is no ColumnPropertySourceConfiguration, MultiColumnPropertySourceConfiguration or FixedValuePropertySourceConfiguration",
                                             conf.Key);
                                }


                                var value = GetValueForConfig(columnPropertySourceConfiguration, csv) ??
                                            GetValueForConfig(multiColumnPropertySourceConfiguration, csv) ??
                                            GetValueForConfig(fixedValuePropertySourceConfiguration);

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

