using System;
using System.Collections.Generic;
using BankDataDownloader.Core.Model;
using BankDataDownloader.Core.Model.FileParser;

namespace BankDataDownloader.Core.DownloadHandler
{
    public interface IBankDownloadHandler : IDisposable
    {
        void Execute(bool cleanupDirectoryBeforeStart);
        void ProcessFiles(IEnumerable<FileParserInput> filesToParse);
    }
}