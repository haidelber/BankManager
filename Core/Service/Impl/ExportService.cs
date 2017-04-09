using System.Collections.Generic;
using System.IO;
using System.Linq;
using BankManager.Core.Model.Account;
using BankManager.Core.Model.Transaction;
using ClosedXML.Excel;


namespace BankManager.Core.Service.Impl
{
    public class ExportService : IExportService
    {
        public IAccountService AccountService { get; }
        public ITransactionService TransactionService { get; }

        public XLWorkbook Workbook { get; set; }

        public ExportService(IAccountService accountService, ITransactionService transactionService)
        {
            AccountService = accountService;
            TransactionService = transactionService;
        }

        public byte[] ExportAllToExcel()
        {
            //https://github.com/closedxml/closedxml/wiki
            Workbook = new XLWorkbook();

            var bankAccounts = AccountService.BankAccounts().ToList();
            var creditCardAccounts = AccountService.CreditCards().ToList();
            var portfolios = AccountService.Portfolios().ToList();
            foreach (var account in bankAccounts.Where(model => model.Active))
            {
                AddSheet(account, TransactionService.GetBankTransactionsForAccountId(account.Id));
            }
            foreach (var account in creditCardAccounts.Where(model => model.Active))
            {
                AddSheet(account, TransactionService.GetCreditCardTransactionsForAccountId(account.Id));
            }
            foreach (var account in portfolios.Where(model => model.Active))
            {
                AddSheet(account, TransactionService.GetPortfolioPositionsForPortfolioId(account.Id));
            }
            foreach (var account in bankAccounts.Where(model => !model.Active))
            {
                AddSheet(account, TransactionService.GetBankTransactionsForAccountId(account.Id));
            }
            foreach (var account in creditCardAccounts.Where(model => !model.Active))
            {
                AddSheet(account, TransactionService.GetCreditCardTransactionsForAccountId(account.Id));
            }
            foreach (var account in portfolios.Where(model => !model.Active))
            {
                AddSheet(account, TransactionService.GetPortfolioPositionsForPortfolioId(account.Id));
            }

            using (var stream = new MemoryStream())
            {
                Workbook.SaveAs(stream);
                return stream.ToArray();
            }
        }
        private int AddHeaderBankTransactionModel(IXLWorksheet worksheet)
        {
            var colIdx = 1;
            worksheet.Column(colIdx).Style.NumberFormat.NumberFormatId = 0;
            worksheet.Cell(1, colIdx++).Value = "AccountId";
            worksheet.Cell(1, colIdx++).Value = "AvailabilityDate";
            worksheet.Cell(1, colIdx++).Value = "PostingDate";
            worksheet.Cell(1, colIdx++).Value = "Text";
            worksheet.Column(colIdx).Style.NumberFormat.NumberFormatId = 44;
            worksheet.Cell(1, colIdx++).Value = "Amount";
            worksheet.Cell(1, colIdx).Value = "CurrencyIso";
            return colIdx;
        }

        private int AddHeaderBankTransactionForeignCurrencyModel(IXLWorksheet worksheet)
        {
            var colIdx = 1;
            worksheet.Column(colIdx).Style.NumberFormat.NumberFormatId = 0;
            worksheet.Cell(1, colIdx++).Value = "AccountId";
            worksheet.Cell(1, colIdx++).Value = "AvailabilityDate";
            worksheet.Cell(1, colIdx++).Value = "PostingDate";
            worksheet.Cell(1, colIdx++).Value = "Text";
            worksheet.Column(colIdx).Style.NumberFormat.NumberFormatId = 44;
            worksheet.Cell(1, colIdx++).Value = "Amount";
            worksheet.Cell(1, colIdx++).Value = "CurrencyIso";
            worksheet.Column(colIdx).Style.NumberFormat.NumberFormatId = 2;
            worksheet.Cell(1, colIdx++).Value = "AmountForeignCurrency";
            worksheet.Cell(1, colIdx++).Value = "ForeignCurrencyIso";
            worksheet.Column(colIdx).Style.NumberFormat.NumberFormatId = 2;
            worksheet.Cell(1, colIdx).Value = "ExchangeRate";
            return colIdx;
        }

        private int AddHeaderPortfolioPositionModel(IXLWorksheet worksheet)
        {
            var colIdx = 1;
            worksheet.Column(colIdx).Style.NumberFormat.NumberFormatId = 0;
            worksheet.Cell(1, colIdx++).Value = "PortfolioId";
            worksheet.Cell(1, colIdx++).Value = "Isin";
            worksheet.Cell(1, colIdx++).Value = "Name";
            worksheet.Column(colIdx).Style.NumberFormat.NumberFormatId = 2;
            worksheet.Cell(1, colIdx++).Value = "Amount";
            worksheet.Cell(1, colIdx++).Value = "DateTime";
            worksheet.Column(colIdx).Style.NumberFormat.NumberFormatId = 44;
            worksheet.Cell(1, colIdx++).Value = "CurrentValue";
            worksheet.Cell(1, colIdx++).Value = "CurrentValueCurrencyIso";
            worksheet.Column(colIdx).Style.NumberFormat.NumberFormatId = 44;
            worksheet.Cell(1, colIdx++).Value = "Current";
            worksheet.Column(colIdx).Style.NumberFormat.NumberFormatId = 44;
            worksheet.Cell(1, colIdx++).Value = "OriginalValue";
            worksheet.Cell(1, colIdx++).Value = "OriginalValueCurrencyIso";
            worksheet.Column(colIdx).Style.NumberFormat.NumberFormatId = 44;
            worksheet.Cell(1, colIdx).Value = "Original";
            return colIdx;
        }

        private void AddRow(IXLWorksheet worksheet, int rowIdx, BankTransactionModel transaction)
        {
            var colIdx = 1;
            worksheet.Cell(rowIdx, colIdx++).Value = transaction.AccountId;
            worksheet.Cell(rowIdx, colIdx++).Value = transaction.AvailabilityDate;
            worksheet.Cell(rowIdx, colIdx++).Value = transaction.PostingDate;
            worksheet.Cell(rowIdx, colIdx++).Value = transaction.Text;
            worksheet.Cell(rowIdx, colIdx++).Value = transaction.Amount;
            worksheet.Cell(rowIdx, colIdx).Value = transaction.CurrencyIso;
        }

        private void AddRow(IXLWorksheet worksheet, int rowIdx, BankTransactionForeignCurrencyModel transaction)
        {
            var colIdx = 1;
            worksheet.Cell(rowIdx, colIdx++).Value = transaction.AccountId;
            worksheet.Cell(rowIdx, colIdx++).Value = transaction.AvailabilityDate;
            worksheet.Cell(rowIdx, colIdx++).Value = transaction.PostingDate;
            worksheet.Cell(rowIdx, colIdx++).Value = transaction.Text;
            worksheet.Cell(rowIdx, colIdx++).Value = transaction.Amount;
            worksheet.Cell(rowIdx, colIdx++).Value = transaction.CurrencyIso;
            worksheet.Cell(rowIdx, colIdx++).Value = transaction.AmountForeignCurrency;
            worksheet.Cell(rowIdx, colIdx++).Value = transaction.ForeignCurrencyIso;
            worksheet.Cell(rowIdx, colIdx).Value = transaction.ExchangeRate;
        }

        private void AddRow(IXLWorksheet worksheet, int rowIdx, PortfolioPositionModel transaction)
        {
            var colIdx = 1;
            worksheet.Cell(rowIdx, colIdx++).Value = transaction.PortfolioId;
            worksheet.Cell(rowIdx, colIdx++).Value = transaction.Isin;
            worksheet.Cell(rowIdx, colIdx++).Value = transaction.Name;
            worksheet.Cell(rowIdx, colIdx++).Value = transaction.Amount;
            worksheet.Cell(rowIdx, colIdx++).Value = transaction.DateTime;
            worksheet.Cell(rowIdx, colIdx++).Value = transaction.CurrentValue;
            worksheet.Cell(rowIdx, colIdx++).Value = transaction.CurrentValueCurrencyIso;
            worksheet.Cell(rowIdx, colIdx++).FormulaR1C1 = $"R{rowIdx}C4*R{rowIdx}C6";
            worksheet.Cell(rowIdx, colIdx++).Value = transaction.OriginalValue;
            worksheet.Cell(rowIdx, colIdx++).Value = transaction.OriginalValueCurrencyIso;
            worksheet.Cell(rowIdx, colIdx).FormulaR1C1 = $"R{rowIdx}C4*R{rowIdx}C9";
        }

        private void AddSheet<TAccount, TTrans>(TAccount account, IEnumerable<TTrans> transactions) where TAccount : AccountModel
        {
            var workSheet = Workbook.Worksheets.Add(GetSheetName(account));
            var rowIndex = 2;
            if (typeof(TTrans) == typeof(BankTransactionModel))
            {
                var colCount = AddHeaderBankTransactionModel(workSheet);
                foreach (var transaction in transactions.Cast<BankTransactionModel>())
                {
                    AddRow(workSheet, rowIndex++, transaction);
                }
                MakeTable(workSheet, 1, colCount, 1, rowIndex - 1, true);
            }
            else if (typeof(TTrans) == typeof(BankTransactionForeignCurrencyModel))
            {
                var colCount = AddHeaderBankTransactionForeignCurrencyModel(workSheet);
                foreach (var transaction in transactions.Cast<BankTransactionForeignCurrencyModel>())
                {
                    AddRow(workSheet, rowIndex++, transaction);
                }
                MakeTable(workSheet, 1, colCount, 1, rowIndex - 1, true);
            }
            else if (typeof(TTrans) == typeof(PortfolioPositionModel))
            {
                var colCount = AddHeaderPortfolioPositionModel(workSheet);
                foreach (var transaction in transactions.Cast<PortfolioPositionModel>())
                {
                    AddRow(workSheet, rowIndex++, transaction);
                }
                MakeTable(workSheet, 1, colCount, 1, rowIndex - 1, true);
            }
        }

        private void MakeTable(IXLWorksheet worksheet, int colStart, int colEnd, int rowStart, int rowEnd, bool hasTableHeader)
        {
            var rngData = worksheet.Range(rowStart, colStart, rowEnd, colEnd);
            var excelTable = rngData.CreateTable();

            excelTable.ShowHeaderRow = hasTableHeader;
            excelTable.ShowTotalsRow = true;
        }

        private string GetSheetName(AccountModel account)
        {
            //if (account is BankAccountModel)
            //{
            //    var typedAccount = account as BankAccountModel;
            //}
            //if (account is CreditCardAccountModel)
            //{
            //    var typedAccount = account as CreditCardAccountModel;
            //}
            //if (account is PortfolioModel)
            //{
            //    var typedAccount = account as PortfolioModel;
            //}
            return $"{account.BankName} {account.AccountName}";
        }

    }
}
