using System.Collections.Generic;
using BankManager.Core.Model.Configuration;

namespace BankManager.Core.Service
{
    public interface IConfigurationService
    {
        KeePassConfigurationModel KeePassConfiguration();
        KeePassConfigurationModel EditKeePassConfiguration(KeePassConfigurationModel model);
        DatabaseConfigurationModel DatabaseConfiguration();
        DatabaseConfigurationModel EditDatabaseConfiguration(DatabaseConfigurationModel model);
        IEnumerable<DownloadHandlerConfigurationModel> DownloadHandlerConfigurations();
        DownloadHandlerConfigurationModel EditDownloadHandlerConfiguration(DownloadHandlerConfigurationModel model);
        DownloadHandlerConfigurationModel DownloadHandlerConfiguration(string key);
    }
}