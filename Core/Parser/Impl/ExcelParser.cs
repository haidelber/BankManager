using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Autofac;
using BankDataDownloader.Common.Model.Configuration;
using Excel;

namespace BankDataDownloader.Core.Parser.Impl
{
    public class ExcelParser : TableParserBase<IExcelDataReader>
    {
        public ExcelParser(IComponentContext context, FileParserConfiguration configuration) : base(context, configuration)
        {
        }

        public override IEnumerable<object> Parse(string filePath)
        {
            using (var file = File.OpenRead(filePath))
            {
                using (IExcelDataReader excelReader = Configuration.ExcelVersion == ExcelVersion.Xls ? ExcelReaderFactory.CreateBinaryReader(file) : ExcelReaderFactory.CreateOpenXmlReader(file))
                {
                    for (var i = 0; i < Configuration.SkipRows; i++)
                    {
                        excelReader.Read();
                    }
                    excelReader.IsFirstRowAsColumnNames = true;

                    while (excelReader.Read())
                    {
                        yield return ParseLine(excelReader);
                    }
                }
            }
        }

        protected override string GetValueByColumnName(IExcelDataReader reader, string columnName)
        {
            return reader[columnName].ToString();
        }

        protected override string GetValueByColumnIndex(IExcelDataReader reader, int columnIndex)
        {
            return reader[columnIndex].ToString();
        }
    }
}