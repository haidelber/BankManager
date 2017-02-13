using Autofac;
using BankDataDownloader.Common;
using BankDataDownloader.Common.Model.Configuration;
using BankDataDownloader.Core.Parser;
using BankDataDownloader.Core.Parser.Impl;
using BankDataDownloader.Core.Service;
using BankDataDownloader.Data.Entity;
using DataDownloader.Test.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace DataDownloader.Test.Autofac
{
    [TestClass]
    public class AutofacTest : ContainerBasedTestBase
    {
        [TestMethod]
        public void TestConfigurationModule()
        {
            var config = ContainerProvider.Container.Resolve<IConfigurationService>();
            IsNotNull(config);
            var appConfig = ContainerProvider.Container.Resolve<ApplicationConfiguration>();
            IsNotNull(appConfig);
            var dbConfig = ContainerProvider.Container.Resolve<DatabaseConfiguration>();
            IsNotNull(dbConfig);
            var keePassConfig = ContainerProvider.Container.Resolve<KeePassConfiguration>();
            IsNotNull(keePassConfig);
            var uiConfig = ContainerProvider.Container.Resolve<UiConfiguration>();
            IsNotNull(uiConfig);
            foreach (var key in Constants.DefaultConfiguration.DownloadHandlerConfigurations.Keys)
            {
                var conf = ContainerProvider.Container.ResolveNamed<DownloadHandlerConfiguration>(key);
                IsNotNull(conf);
            }
            foreach (var key in Constants.DefaultConfiguration.FileParserConfiguration.Keys)
            {
                var conf = ContainerProvider.Container.ResolveNamed<FileParserConfiguration>(key);
                IsNotNull(conf);
            }
        }

        [TestMethod]
        public void TestValueParserModule()
        {
            var parser = ContainerProvider.Container.ResolveNamed<IValueParser>(Constants.UniqueContainerKeys.ValueParserDateTime);
            IsNotNull(parser);
            parser = ContainerProvider.Container.ResolveNamed<IValueParser>(Constants.UniqueContainerKeys.ValueParserDateTimeExact, new NamedParameter("format", "yyyy-MM-dd"));
            IsNotNull(parser);
            parser = ContainerProvider.Container.ResolveNamed<IValueParser>(Constants.UniqueContainerKeys.ValueParserEnglishDecimal);
            IsNotNull(parser);
            parser = ContainerProvider.Container.ResolveNamed<IValueParser>(Constants.UniqueContainerKeys.ValueParserGermanDecimal);
            IsNotNull(parser);
            parser = ContainerProvider.Container.ResolveNamed<IValueParser>(Constants.UniqueContainerKeys.ValueParserString);
            IsNotNull(parser);
            parser = ContainerProvider.Container.ResolveNamed<IValueParser>(Constants.UniqueContainerKeys.ValueParserEnum, new NamedParameter("targetType", typeof(ValueParser)));
            IsNotNull(parser);
        }

        [TestMethod]
        public void TestParserModule()
        {
            var parser = Container.Resolve<CsvParser<RaiffeisenTransactionEntity>>();
            IsNotNull(parser);
        }
    }
}
