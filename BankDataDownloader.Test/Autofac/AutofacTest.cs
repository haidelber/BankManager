using System.Data.Entity;
using Autofac;
using BankDataDownloader.Common;
using BankDataDownloader.Common.Model.Configuration;
using BankDataDownloader.Core;
using BankDataDownloader.Core.Configuration;
using BankDataDownloader.Core.DownloadHandler.Impl;
using BankDataDownloader.Core.Parser;
using BankDataDownloader.Core.Service;
using BankDataDownloader.Core.ValueProvider;
using BankDataDownloader.Data;
using BankDataDownloader.Data.Entity;
using BankDataDownloader.Data.Entity.BankTransactions;
using BankDataDownloader.Data.Repository;
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
            foreach (var key in DefaultConfigurations.ApplicationConfiguration.DownloadHandlerConfigurations.Keys)
            {
                var conf = ContainerProvider.Container.ResolveNamed<DownloadHandlerConfiguration>(key);
                IsNotNull(conf);
            }
            foreach (var key in DefaultConfigurations.ApplicationConfiguration.FileParserConfiguration.Keys)
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
            var parser = Container.ResolveNamed<IFileParser>(Constants.UniqueContainerKeys.FileParserRaiffeisen);
            IsNotNull(parser);
        }

        [TestMethod]
        public void TestDataModule()
        {
            var dbContext = Container.Resolve<DbContext>();
            IsNotNull(dbContext);

            var equality = Container.Resolve<IEntityEqualityComparer<RaiffeisenTransactionEntity>>();
            IsNotNull(equality);

            var bankAccountRepo = Container.Resolve<IBankAccountRepository>();
            IsNotNull(bankAccountRepo);
            var bankAccountRepo2 = Container.Resolve<IRepository<BankAccountEntity>>();
            IsNotNull(bankAccountRepo2);
            var transRepo2 = Container.Resolve<IRepository<RaiffeisenTransactionEntity>>();
            IsNotNull(transRepo2);
            var transRepo3 = Container.Resolve<IRepository<Number26TransactionEntity>>();
            IsNotNull(transRepo3);
        }

        [TestMethod]
        public void TestServices()
        {
            var passProv = Container.Resolve<IKeePassPasswordValueProvider>();
            IsNotNull(passProv);
            var keePass = Container.Resolve<IKeePassService>();
            IsNotNull(keePass);
            var conf = Container.Resolve<IConfigurationService>();
            IsNotNull(conf);
        }

        [TestMethod]
        public void TestDownloadHandler()
        {
            var raiffeisen = Container.Resolve<RaiffeisenDownloadHandler>();
            IsNotNull(raiffeisen);
        }
    }
}
