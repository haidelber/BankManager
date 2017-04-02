using System.IO;
using Autofac.Core;
using BankDataDownloader.Common.Model.Configuration;

namespace BankDataDownloader.Core.Service
{
    public interface IConfigurationService : IModule
    {
        //ApplicationConfiguration GetApplicationConfiguration();
        //DownloadHandlerConfiguration GetDownloadHandlerConfiguration(string downloadHandlerKey);
        //KeePassConfiguration GetKeePassConfiguration();
        //UiConfiguration GetUiConfiguration();

        void ImportConfiguration(Stream source);
        void ExportConfiguration(Stream destination);
        ApplicationConfiguration ApplicationConfiguration { get; }
        string ConfigurationFilePath { get; }
        void SaveConfiguration(ApplicationConfiguration configuration);
    }
}