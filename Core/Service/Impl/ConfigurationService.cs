using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BankManager.Common.Model.Configuration;
using BankManager.Core.Model.Configuration;

namespace BankManager.Core.Service.Impl
{
    public class ConfigurationService : IConfigurationService
    {
        public Provider.IConfigurationProvider ConfigurationProvider { get; }
        public IMapper Mapper { get; }

        public ConfigurationService(Provider.IConfigurationProvider configurationProvider, IMapper mapper)
        {
            ConfigurationProvider = configurationProvider;
            Mapper = mapper;
        }

        private void SaveConfiguration()
        {
            ConfigurationProvider.SaveConfiguration();
        }

        public KeePassConfigurationModel KeePassConfiguration()
        {
            var model =
                Mapper.Map<KeePassConfigurationModel>(ConfigurationProvider.ApplicationConfiguration.KeePassConfiguration);
            return model;
        }

        public KeePassConfigurationModel EditKeePassConfiguration(KeePassConfigurationModel model)
        {
            Mapper.Map(model, ConfigurationProvider.ApplicationConfiguration.KeePassConfiguration);
            SaveConfiguration();
            return KeePassConfiguration();
        }

        public DatabaseConfigurationModel DatabaseConfiguration()
        {
            var model =
                Mapper.Map<DatabaseConfigurationModel>(ConfigurationProvider.ApplicationConfiguration.DatabaseConfiguration);
            return model;
        }

        public DatabaseConfigurationModel EditDatabaseConfiguration(DatabaseConfigurationModel model)
        {
            Mapper.Map(model, ConfigurationProvider.ApplicationConfiguration.DatabaseConfiguration);
            SaveConfiguration();
            return DatabaseConfiguration();
        }

        public IEnumerable<DownloadHandlerConfigurationModel> DownloadHandlerConfigurations()
        {
            var model = ConfigurationProvider.ApplicationConfiguration.DownloadHandlerConfigurations.Select(ToDownloadHandlerConfigurationModel);
            return model;
        }

        public DownloadHandlerConfigurationModel EditDownloadHandlerConfiguration(DownloadHandlerConfigurationModel model)
        {
            Mapper.Map(model, ConfigurationProvider.ApplicationConfiguration.DownloadHandlerConfigurations[model.Key]);
            SaveConfiguration();
            return DownloadHandlerConfiguration(model.Key);
        }

        public DownloadHandlerConfigurationModel DownloadHandlerConfiguration(string key)
        {
            var model = ToDownloadHandlerConfigurationModel(key, ConfigurationProvider.ApplicationConfiguration.DownloadHandlerConfigurations[key]);
            return model;
        }

        private DownloadHandlerConfigurationModel ToDownloadHandlerConfigurationModel(KeyValuePair<string, DownloadHandlerConfiguration> pair)
        {
            return ToDownloadHandlerConfigurationModel(pair.Key, pair.Value);
        }

        private DownloadHandlerConfigurationModel ToDownloadHandlerConfigurationModel(string key, DownloadHandlerConfiguration conf)
        {
            var singleConf = Mapper.Map<DownloadHandlerConfigurationModel>(conf);
            singleConf.Key = key;
            return singleConf;
        }
    }
}
