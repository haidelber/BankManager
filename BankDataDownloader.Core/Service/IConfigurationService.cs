using System.IO;
using BankDataDownloader.Common.Model.Configuration;

namespace BankDataDownloader.Core.Service
{
    public interface IConfigurationService
    {
        ApplicationConfiguration GetApplicationConfiguration();
        DownloadHandlerConfiguration GetDownloadHandlerConfiguration(string downloadHandlerKey);
        KeePassConfiguration GetKeePassConfiguration();
        UiConfiguration GetUiConfiguration();

        void ImportConfiguration(Stream source);
        void ExportConfiguration(Stream destination);
    }
}