using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankDataDownloader.Common.Model.Configuration;
using CsvHelper;
using CsvHelper.Configuration;

namespace BankDataDownloader.Core.Parser.Impl
{
    public class CsvParser<TTarget> : IFileParser<TTarget> where TTarget : new()
    {
        public FileParserConfiguration Configuration { get; set; }

        public CsvParser(FileParserConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IEnumerable<TTarget> Parse(string filePath)
        {
            using (var file = File.OpenText(filePath))
            {
                var csvConf = new CsvConfiguration {HasHeaderRecord = Configuration.HasHeaderRow};
                using (var csv = new CsvReader(file, csvConf))
                {
                    for(var i = 0; i < Configuration.SkipRows, i++)
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
                            object value = null;
                            //TODO continue parsing
                            var prop = typeof(TTarget).GetProperty(conf.Key);
                            prop.SetValue(target,value);
                        }
                        

                        yield return target;
                    }
                }
            }
        }
    }
}
