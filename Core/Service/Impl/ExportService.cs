using System;
using System.Collections.Generic;
using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace BankManager.Core.Service.Impl
{
    public class ExportService : IExportService
    {
        public byte[] ExportAllToExcel()
        {
            //TODO https://www.codeproject.com/Articles/670141/Read-and-Write-Microsoft-Excel-with-Open-XML-SDK
            // https://msdn.microsoft.com/en-us/library/office/hh180830(v=office.14).aspx
            var stream = new MemoryStream();
            var document = SpreadsheetDocument
                .Create(stream, SpreadsheetDocumentType.Workbook);

            var workbookpart = document.AddWorkbookPart();
            workbookpart.Workbook = new Workbook();
            var worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
            var sheetData = new SheetData();

            worksheetPart.Worksheet = new Worksheet(sheetData);

            var sheets = document.WorkbookPart.Workbook.
                AppendChild<Sheets>(new Sheets());

            var sheet = new Sheet()
            {
                Id = document.WorkbookPart
                .GetIdOfPart(worksheetPart),
                SheetId = 1,
                Name = "First Account"
            };
            sheets.AppendChild(sheet);

            uint rowIdex = 0;
            var row = new Row { RowIndex = ++rowIdex };
            sheetData.AppendChild(row);

            foreach (var rowData in new List<string> { "Hello", "from", ".Net", "Core" })
            {
                var cellIdex = 0;
                row = new Row { RowIndex = ++rowIdex };
                sheetData.AppendChild(row);
                var cell = CreateTextCell(ColumnLetter(cellIdex++),
                         rowIdex, rowData ?? string.Empty);
                row.AppendChild(cell);
            }

            workbookpart.Workbook.Save();
            document.Close();
            return stream.ToArray();
        }

        private Cell CreateTextCell(string header, UInt32 index,
            string text)
        {
            var cell = new Cell
            {
                DataType = CellValues.InlineString,
                CellReference = header + index
            };

            var istring = new InlineString();
            var t = new Text { Text = text };
            istring.AppendChild(t);
            cell.AppendChild(istring);
            return cell;
        }

        private string ColumnLetter(int intCol)
        {
            var intFirstLetter = ((intCol) / 676) + 64;
            var intSecondLetter = ((intCol % 676) / 26) + 64;
            var intThirdLetter = (intCol % 26) + 65;

            var firstLetter = (intFirstLetter > 64)
                ? (char)intFirstLetter : ' ';
            var secondLetter = (intSecondLetter > 64)
                ? (char)intSecondLetter : ' ';
            var thirdLetter = (char)intThirdLetter;

            return string.Concat(firstLetter, secondLetter,
                thirdLetter).Trim();
        }
    }
}
