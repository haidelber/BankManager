using System;

namespace BankDataDownloader.Core.DownloadHandler.Interfaces
{
    public interface IBankDownloadHandler : IDisposable
    {
        void DownloadAllData();
    }
}