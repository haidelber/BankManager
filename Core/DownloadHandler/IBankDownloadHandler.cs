using System;
using System.Collections.Generic;
using BankManager.Core.Model.FileParser;

namespace BankManager.Core.DownloadHandler
{
    public interface IBankDownloadHandler : IDisposable
    {
        void Execute(bool cleanupDirectoryBeforeStart);
        void ProcessFiles(IEnumerable<FileParserInput> filesToParse);
    }
}