using System.IO;
using Autofac.Core;
using BankManager.Common.Model.Configuration;

namespace BankManager.Core.Provider
{
    public interface IConfigurationProvider : IModule
    {
        //ApplicationConfiguration GetApplicationConfiguration();
        //DownloadHandlerConfiguration GetDownloadHandlerConfiguration(string downloadHandlerKey);
        //KeePassConfiguration GetKeePassConfiguration();
        //UiConfiguration GetUiConfiguration();

        void ImportConfiguration(Stream source);
        void ExportConfiguration(Stream destination);
        ApplicationConfiguration ApplicationConfiguration { get; }
        string ConfigurationFilePath { get; }
        void SaveConfiguration(ApplicationConfiguration configuration = null);
        string ExportConfiguration();
    }
}