using System.Collections.Generic;

namespace BankDataDownloader.Core.Service
{
    public interface ILogService
    {
        IEnumerable<string> GetLogFilePaths();
        string GetLogs(string path);
    }
}
