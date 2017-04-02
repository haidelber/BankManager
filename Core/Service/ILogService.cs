using System.Collections.Generic;

namespace BankManager.Core.Service
{
    public interface ILogService
    {
        IEnumerable<string> GetLogFilePaths();
        string GetLogs(string path);
    }
}
