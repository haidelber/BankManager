using System.Collections.Generic;
using System.IO;
using Autofac;
using BankDataDownloader.Common.Model.Configuration;
using CsvHelper;
using CsvHelper.Configuration;

namespace BankDataDownloader.Core.Parser.Impl
{
    public class CsvParser : TableParserBase<CsvReader>
    {
        public CsvParser(IComponentContext context, FileParserConfiguration configuration) : base(context, configuration)
        {
        }

        public override IEnumerable<object> Parse(Stream input)
        {
            using (var text = new StreamReader(input, Configuration.Encoding))
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
                        yield return ParseLine(csv);
                    }
                }
            }
        }

        protected override string GetValueByColumnName(CsvReader reader, string columnName)
        {
            return reader[columnName];
        }

        protected override string GetValueByColumnIndex(CsvReader reader, int columnIndex)
        {
            return reader[columnIndex];
        }
    }
}

