using System;
using System.IO;
using System.Text;
using Autofac;
using BankDataDownloader.Common;
using BankDataDownloader.Common.Extensions;
using BankDataDownloader.Common.Model.Configuration;
using BankDataDownloader.Core.Extension;
using BankDataDownloader.Core.Service;
using Newtonsoft.Json;

namespace BankDataDownloader.Core.Configuration
{
    public class ConfigurationModule : Module, IConfigurationService
    {
        //TODO make application portable by not writing to users appdata
        public string ConfigurationFilePath { get; } =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                Constants.AppDataSubfolder, Constants.AppConfigFileName);
        public ApplicationConfiguration ApplicationConfiguration { get; private set; }

        protected override void Load(ContainerBuilder builder)
        {
            LoadConfigurationFromFile();

            builder.RegisterInstance(this).As<IConfigurationService>().SingleInstance();
            builder.RegisterAdapter<IConfigurationService, ApplicationConfiguration>(
                service => service.ApplicationConfiguration).SingleInstance();
            builder.RegisterAdapter<IConfigurationService, KeePassConfiguration>(
                service => service.ApplicationConfiguration.KeePassConfiguration).SingleInstance();
            builder.RegisterAdapter<IConfigurationService, DatabaseConfiguration>(
                service => service.ApplicationConfiguration.DatabaseConfiguration).SingleInstance();
            builder.RegisterAdapter<IConfigurationService, UiConfiguration>(
                service => service.ApplicationConfiguration.UiConfiguration).SingleInstance();
            //TODO this only loads config already available at application start
            foreach (var configuration in ApplicationConfiguration.DownloadHandlerConfigurations)
            {
                builder.RegisterInstance(configuration.Value)
                    .Named<DownloadHandlerConfiguration>(configuration.Key)
                    .SingleInstance();
            }
            foreach (var configuration in ApplicationConfiguration.FileParserConfiguration)
            {
                builder.RegisterInstance(configuration.Value)
                    .Named<FileParserConfiguration>(configuration.Key)
                    .SingleInstance();
            }
        }

        private ApplicationConfiguration LoadConfigurationFromFile()
        {
            if (File.Exists(ConfigurationFilePath))
            {
                using (var file = File.OpenRead(ConfigurationFilePath))
                {
                    ImportConfiguration(file);
                }
            }
            else
            {
                ApplicationConfiguration = DefaultConfigurations.ApplicationConfiguration;
            }
            return ApplicationConfiguration;
        }

        public void SaveConfiguration(ApplicationConfiguration configuration)
        {
            BackupOldConfigurationFile();
            ApplyNewConfiguration(configuration);
            using (var file = File.OpenWrite(ConfigurationFilePath))
            {
                ExportConfiguration(file);
            }
        }

        private void ApplyNewConfiguration(ApplicationConfiguration configuration)
        {
            ApplicationConfiguration = configuration;
            //TODO depending on scope of configuration items we need to rebuild the container here
        }

        private void BackupOldConfigurationFile()
        {
            var dir = Path.GetDirectoryName(ConfigurationFilePath);
            var file = Path.GetFileNameWithoutExtension(ConfigurationFilePath);
            var extension = Path.GetExtension(ConfigurationFilePath);
            var suffix = DateTime.Now.ToSortableFileName();
            var newFileName = $"{file}.{suffix}.{extension}";
            File.Move(ConfigurationFilePath, Path.Combine(dir, newFileName));
        }

        public void ImportConfiguration(Stream source)
        {
            using (StreamReader streamReader = new StreamReader(source, Encoding.UTF8))
            {
                using (var jsonTextReader = new JsonTextReader(streamReader))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    var configuration = serializer.Deserialize<ApplicationConfiguration>(jsonTextReader);
                    ApplyNewConfiguration(configuration);
                }
            }

        }

        public void ExportConfiguration(Stream destination)
        {
            using (var streamWriter = new StreamWriter(destination, Encoding.UTF8))
            {
                using (var jsonTextWriter = new JsonTextWriter(streamWriter))
                {
                    jsonTextWriter.Formatting = Formatting.Indented;
                    var serializer = new JsonSerializer();
                    serializer.Serialize(jsonTextWriter, ApplicationConfiguration);
                }
            }
        }
    }
}
