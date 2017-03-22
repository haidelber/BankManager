using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankDataDownloader.Core.Service
{
    public interface ILogService
    {
        IEnumerable<string> GetLogFilePaths();
        string GetLogs(string path);
    }
}
