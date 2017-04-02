using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Autofac;
using BankManager.Common.Model.Configuration;
using Excel;

namespace BankManager.Core.Parser.Impl
{
    public class ExcelParser : TableParserBase<DataRow>
    {
        public ExcelParser(IComponentContext context, FileParserConfiguration configuration) : base(context, configuration)
        {
        }

        public override IEnumerable<object> Parse(Stream input)
        {
            using (var excelReader = Configuration.ExcelVersion == ExcelVersion.Xls ? ExcelReaderFactory.CreateBinaryReader(input) : ExcelReaderFactory.CreateOpenXmlReader(input))
            {
                excelReader.IsFirstRowAsColumnNames = Configuration.HasHeaderRow;
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