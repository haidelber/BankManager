using Autofac;
using BankManager.Common;
using BankManager.Common.Model.Configuration;
using BankManager.Core.Configuration;
using BankManager.Core.DownloadHandler.Impl;
using BankManager.Core.Parser;
using BankManager.Core.Service;
using BankManager.Core.ValueProvider;
using BankManager.Data;
using BankManager.Data.Entity;
using BankManager.Data.Entity.BankTransactions;
using BankManager.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BankManager.Test.Autofac
{
    [TestClass]
    public class AutofacTest : ContainerBasedTestBase
    {
        [TestMethod]
        public void TestConfigurationModule()
        {
            var config = ContainerProvider.Container.Resolve<IConfigurationService>();
            Assert.IsNotNull(config);
            var appConfig = ContainerProvider.Container.Resolve<ApplicationConfiguration>();
            Assert.IsNotNull(appConfig);
            var dbConfig = ContainerProvider.Container.Resolve<DatabaseConfiguration>();
            Assert.IsNotNull(dbConfig);
            var keePassConfig = ContainerProvider.Container.Resolve<KeePassConfiguration>();
            Assert.IsNotNull(keePassConfig);
            var uiConfig = ContainerProvider.Container.Resolve<UiConfiguration>();
            Assert.IsNotNull(uiConfig);
            foreach (var key in DefaultConfigurations.ApplicationConfiguration.DownloadHandlerConfigurations.Keys)
            {
                var conf = ContainerProvider.Container.ResolveKeyed<DownloadHandlerConfiguration>(key);
                Assert.IsNotNull(conf);
            }
            foreach (var key in DefaultConfigurations.ApplicationConfiguration.FileParserConfigurations.Keys)
            {
                var conf = ContainerProvider.Container.ResolveKeyed<FileParserConfiguration>(key);
                Assert.IsNotNull(conf);
            }
        }

        [TestMethod]
        public void TestValueParserModule()
        {
            var parser = ContainerProvider.Container.ResolveKeyed<IValueParser>(Constants.UniqueContainerKeys.ValueParserDateTime);
            Assert.IsNotNull(parser);
            parser = ContainerProvider.Container.ResolveKeyed<IValueParser>(Constants.UniqueContainerKeys.ValueParserDateTimeExact, new NamedParameter("format", "yyyy-MM-dd"));
            Assert.IsNotNull(parser);
            parser = ContainerProvider.Container.ResolveKeyed<IValueParser>(Constants.UniqueContainerKeys.ValueParserEnglishDecimal);
            Assert.IsNotNull(parser);
            parser = ContainerProvider.Container.ResolveKeyed<IValueParser>(Constants.UniqueContainerKeys.ValueParserGermanDecimal);
            Assert.IsNotNull(parser);
            parser = ContainerProvider.Container.ResolveKeyed<IValueParser>(Constants.UniqueContainerKeys.ValueParserString);
            Assert.IsNotNull(parser);
            parser = ContainerProvider.Container.ResolveKeyed<IValueParser>(Constants.UniqueContainerKeys.ValueParserEnum, new NamedParameter("targetType", typeof(ValueParser)));
            Assert.IsNotNull(parser);
        }

        [TestMethod]
        public void TestParserModule()
        {
            var parser = Container.ResolveKeyed<IFileParser>(Constants.UniqueContainerKeys.FileParserRaiffeisenGiro);
            Assert.IsNotNull(parser);
        }

        [TestMethod]
        public void TestDataModule()
        {
            var dbContext = Container.Resolve<DbContext>();
            Assert.IsNotNull(dbContext);

            var equality = Container.Resolve<IEntityEqualityComparer<RaiffeisenTransactionEntity>>();
            Assert.IsNotNull(equality);

            var bankAccountRepo = Container.Resolve<IBankAccountRepository>();
            Assert.IsNotNull(bankAccountRepo);
            var bankAccountRepo2 = Container.Resolve<IRepository<BankAccountEntity>>();
            Assert.IsNotNull(bankAccountRepo2);
            var transRepo2 = Container.Resolve<IRepository<RaiffeisenTransactionEntity>>();
            Assert.IsNotNull(transRepo2);
            var transRepo3 = Container.Resolve<IRepository<Number26TransactionEntity>>();
            Assert.IsNotNull(transRepo3);
        }

        [TestMethod]
        public void TestServices()
        {
            var passProv = Container.Resolve<IKeePassPasswordValueProvider>();
            Assert.IsNotNull(passProv);
            var keePass = Container.Resolve<IKeePassService>();
            Assert.IsNotNull(keePass);
            var conf = Container.Resolve<IConfigurationService>();
            Assert.IsNotNull(conf);
        }

        [TestMethod]
        public void TestDownloadHandler()
        {
            var raiffeisen = Container.Resolve<RaiffeisenDownloadHandler>();
            Assert.IsNotNull(raiffeisen);
        }
    }
}
