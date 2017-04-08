using System;
using BankManager.Common.Extensions;
using BankManager.Core.Service;
using Microsoft.AspNetCore.Mvc;

namespace BankManager.Ui.Controllers
{
    public class ExportController : ApiController
    {
        public IExportService ExportService { get; }

        public ExportController(IExportService exportService)
        {
            ExportService = exportService;
        }

        [HttpGet("Excel")]
        public FileResult ExportToExcel()
        {
            var fileContent = ExportService.ExportAllToExcel();
            return File(fileContent, "application/vnd.ms-excel",
                $"{DateTime.Now.ToSortableFileName()}_BankManager_Export.xlsx");
        }
    }
}
