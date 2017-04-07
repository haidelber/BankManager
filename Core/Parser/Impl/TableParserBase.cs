using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Autofac;
using BankManager.Common.Model.Configuration;
using BankManager.Core.Extension;

namespace BankManager.Core.Parser.Impl
{
    public abstract class TableParserBase<TReader> : IFileParser
    {
        public FileParserConfiguration Configuration { get; }
        public IComponentContext Context { get; }

        protected TableParserBase(IComponentContext context, FileParserConfiguration configuration)
        {
            Context = context;
            Configuration = configuration;
        }

        protected object GetValueForConfig(ColumnPropertySourceConfiguration config, TReader tableReader)
        {
            if (config == null)
                return null;
            string rawValue = null;
            if (Configuration.HasHeaderRow && config.ColumnName != null)
            {
                rawValue = GetValueByColumnName(tableReader, config.ColumnName);
            }
            else if (config.ColumnIndex.HasValue)
            {
                rawValue = GetValueByColumnIndex(tableReader, config.ColumnIndex.Value);
            }
            else throw new ArgumentException(
               "No column index given although name couldn't be used as index or index is prefered",
               "ColumnIndex");
            Debug.Assert(config.ColumnIndex != null, "config.ColumnIndex != null");

            var parser = config.ResolveParser(Context);
            return parser.Parse(rawValue);
        }

        protected abstract string GetValueByColumnName(TReader reader, string columnName);
        protected abstract string GetValueByColumnIndex(TReader reader, int columnIndex);

        protected object GetValueForConfig(MultiColumnPropertySourceConfiguration config, TReader tableReader)
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
                    rawValue = GetValueByColumnName(tableReader, configColumnName);
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
                            rawValue = GetValueByColumnIndex(tableReader, columnIndex.Value);
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

        protected object GetValueForConfig(FixedValuePropertySourceConfiguration config)
        {
            var parser = config?.ResolveParser(Context);
            return parser?.Parse(config.FixedValue);
        }

        protected object ParseLine(TReader reader)
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


                var value = GetValueForConfig(columnPropertySourceConfiguration, reader) ??
                            GetValueForConfig(multiColumnPropertySourceConfiguration, reader) ??
                            GetValueForConfig(fixedValuePropertySourceConfiguration);

                var prop = Configuration.TargetType.GetProperty(conf.Key);
                prop.SetValue(target, value);
            }
            return target;
        }

        public IEnumerable<object> Parse(string filePath)
        {
            using (var file = File.OpenRead(filePath))
            {
                return Parse(file).ToList();
            }
        }
        public abstract IEnumerable<object> Parse(Stream input);
    }
}