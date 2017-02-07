using System;

namespace BankDataDownloader.Core.DownloadHandler
{
    public interface IBankDownloadHandler : IDisposable
    {
        void Execute(bool cleanupDirectoryBeforeStart);
    }
}