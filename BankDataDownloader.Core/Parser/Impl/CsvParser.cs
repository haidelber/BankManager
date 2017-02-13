using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using BankDataDownloader.Common.Extensions;
using BankDataDownloader.Common.Model.Configuration;
using BankDataDownloader.Core.Extension;
using CsvHelper;
using CsvHelper.Configuration;

namespace BankDataDownloader.Core.Parser.Impl
{
    public class CsvParser<TTarget> : IFileParser<TTarget> where TTarget : new()
    {
        public FileParserConfiguration Configuration { get; }
        public IComponentContext Context { get; }

        public CsvParser(FileParserConfiguration configuration, IComponentContext context)
        {
            Configuration = configuration;
            Context = context;
        }

        public IEnumerable<TTarget> Parse(string filePath)
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
                            var target = new TTarget();
                            foreach (var conf in Configuration.PropertySourceConfiguration)
                            {
                                var tableLikeConfig = conf.Value as TableLikePropertySourceConfiguration;
                                if (tableLikeConfig == null)
                                {
                                    throw new ArgumentException(
                                        "PropertySourceConfiguration is no TableLikePropertySourceConfiguration",
                                        conf.Key);
                                }
                                var parser = tableLikeConfig.ResolveParser(Context);
                                string rawValue = null;
                                if (Configuration.HasHeaderRow)
                                {
                                    rawValue = csv[tableLikeConfig.ColumnName];
                                }
                                if (rawValue == null)
                                {
                                    if (!tableLikeConfig.ColumnIndex.HasValue)
                                    {
                                        throw new ArgumentException(
                                            "No column index given although name couldn't be used as index or index is prefered",
                                            "ColumnIndex");
                                    }
                                    rawValue = csv[tableLikeConfig.ColumnIndex.Value];
                                }

                                var value = parser.Parse(rawValue);
                                var prop = typeof(TTarget).GetProperty(conf.Key);
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

