using System.IO;

namespace BankManager.Core.Service
{
    public interface IExportService
    {
        byte[] ExportAllToExcel();
    }
}