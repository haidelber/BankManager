using Autofac;
using BankManager.Common;
using BankManager.Common.Model.Configuration;
using BankManager.Core.Configuration;
using BankManager.Core.DownloadHandler.Impl;
using BankManager.Core.Parser;
using BankManager.Core.Provider;
using BankManager.Core.Service;
using BankManager.Data;
using BankManager.Data.Entity;
using BankManager.Data.Entity.BankTransactions;
using BankManager.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace BankManager.Test.Autofac
{
    [TestClass]
    public class AutofacTest : ContainerBasedTestBase
    {
        [TestMethod]
        public void TestConfigurationModule()
        {
            var config = ContainerProvider.Container.Resolve<IConfigurationProvider>();
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
                var conf = ContainerProvider.Container.ResolveKeyed<DownloadHandlerConfiguration>(key);
                IsNotNull(conf);
            }
            foreach (var key in DefaultConfigurations.ApplicationConfiguration.FileParserConfigurations.Keys)
            {
                var conf = ContainerProvider.Container.ResolveKeyed<FileParserConfiguration>(key);
                IsNotNull(conf);
            }
        }

        [TestMethod]
        public void TestValueParserModule()
        {
            var parser = ContainerProvider.Container.ResolveKeyed<IValueParser>(Constants.UniqueContainerKeys.ValueParserDateTime);
            IsNotNull(parser);
            parser = ContainerProvider.Container.ResolveKeyed<IValueParser>(Constants.UniqueContainerKeys.ValueParserDateTimeExact, new NamedParameter("format", "yyyy-MM-dd"));
            IsNotNull(parser);
            parser = ContainerProvider.Container.ResolveKeyed<IValueParser>(Constants.UniqueContainerKeys.ValueParserEnglishDecimal);
            IsNotNull(parser);
            parser = ContainerProvider.Container.ResolveKeyed<IValueParser>(Constants.UniqueContainerKeys.ValueParserGermanDecimal);
            IsNotNull(parser);
            parser = ContainerProvider.Container.ResolveKeyed<IValueParser>(Constants.UniqueContainerKeys.ValueParserString);
            IsNotNull(parser);
            parser = ContainerProvider.Container.ResolveKeyed<IValueParser>(Constants.UniqueContainerKeys.ValueParserEnum, new NamedParameter("targetType", typeof(ValueParser)));
            IsNotNull(parser);
        }

        [TestMethod]
        public void TestParserModule()
        {
            var parser = Container.ResolveKeyed<IFileParser>(Constants.UniqueContainerKeys.FileParserRaiffeisenGiro);
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
            var passProv = Container.Resolve<IKeePassPasswordProvider>();
            IsNotNull(passProv);
            var keePass = Container.Resolve<IKeePassService>();
            IsNotNull(keePass);
            var conf = Container.Resolve<IConfigurationProvider>();
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
