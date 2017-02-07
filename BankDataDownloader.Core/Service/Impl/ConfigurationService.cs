using System;
using System.IO;
using BankDataDownloader.Common.Model.Configuration;

namespace BankDataDownloader.Core.Service.Impl
{
    public class ConfigurationService:IConfigurationService
    {
        public ApplicationConfiguration GetApplicationConfiguration()
        {
            throw new NotImplementedException();
        }

        public DownloadHandlerConfiguration GetDownloadHandlerConfiguration(string downloadHandlerKey)
        {
            throw new NotImplementedException();
        }

        public KeePassConfiguration GetKeePassConfiguration()
        {
            throw new NotImplementedException();
        }

        public UiConfiguration GetUiConfiguration()
        {
            throw new NotImplementedException();
        }

        public void ImportConfiguration(Stream source)
        {
            throw new NotImplementedException();
        }

        public void ExportConfiguration(Stream destination)
        {
            throw new NotImplementedException();
        }
    }
}
