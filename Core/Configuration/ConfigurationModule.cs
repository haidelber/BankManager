using System;
using System.IO;
using System.Text;
using Autofac;
using Autofac.Core;
using BankDataDownloader.Common;
using BankDataDownloader.Common.Converter;
using BankDataDownloader.Common.Extensions;
using BankDataDownloader.Common.Model.Configuration;
using BankDataDownloader.Core.Parser;
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

        public JsonSerializer JsonSerializer { get; }

        public ConfigurationModule()
        {
            var cb = new ContainerBuilder();
            cb.RegisterModule<DefaultJsonModule>();
            JsonSerializer = cb.Build().Resolve<JsonSerializer>();
        }

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
            foreach (var configuration in ApplicationConfiguration.FileParserConfigurations)
            {
                builder.RegisterInstance(configuration.Value)
                    .Named<FileParserConfiguration>(configuration.Key)
                    .SingleInstance();
                builder.RegisterType(configuration.Value.ParserType)
                .WithParameter(
                    new ResolvedParameter(
                        (param, ctx) => param.ParameterType == typeof(FileParserConfiguration),
                        (pi, context) =>
                            context.ResolveNamed<FileParserConfiguration>(configuration.Key)))
                .Named<IFileParser>(configuration.Key);
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
                    var configuration = JsonSerializer.Deserialize<ApplicationConfiguration>(jsonTextReader);
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
                    JsonSerializer.Serialize(jsonTextWriter, ApplicationConfiguration);
                }
            }
        }
    }
}
