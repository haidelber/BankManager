using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Autofac;
using BankDataDownloader.Common.Model.Configuration;
using Excel;

namespace BankDataDownloader.Core.Parser.Impl
{
    public class ExcelParser : TableParserBase<DataRow>
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
                    var dataSet = excelReader.AsDataSet();
                    DataTable dataTable = null;
                    if (Configuration.TableName != null)
                    {
                        dataTable = dataSet.Tables[Configuration.TableName];
                    }
                    else if (Configuration.TableIndex.HasValue)
                    {
                        dataTable = dataSet.Tables[Configuration.TableIndex.Value];
                    }
                    else throw new ArgumentException("The Excel Parser needs either a TableName or a TableIndex to work", "TableName or TableIndex");
                    foreach (DataRow row in dataTable.Rows)
                    {
                        yield return ParseLine(row);
                    }
                }
            }
        }

        protected override string GetValueByColumnName(DataRow row, string columnName)
        {
            return row[columnName].ToString();
        }

        protected override string GetValueByColumnIndex(DataRow row, int columnIndex)
        {
            return row[columnIndex].ToString();
        }
    }
}