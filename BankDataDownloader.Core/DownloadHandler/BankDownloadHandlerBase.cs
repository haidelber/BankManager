using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using Autofac;
using BankDataDownloader.Common.Extensions;
using BankDataDownloader.Common.Model.Configuration;
using BankDataDownloader.Core.Extension;
using BankDataDownloader.Core.Parser;
using BankDataDownloader.Core.Service;
using BankDataDownloader.Core.Service.Impl;
using BankDataDownloader.Data.Entity;
using BankDataDownloader.Data.Repository;
using BankDataDownloader.Data.Repository.Impl;
using KeePassLib;
using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace BankDataDownloader.Core.DownloadHandler
{
    public abstract class BankDownloadHandlerBase : IBankDownloadHandler
    {
        public readonly Logger Log = LogManager.GetCurrentClassLogger();

        public IBankAccountRepository BankAccountRepository { get; }
        public DbContext DbContext { get; set; }
        public IKeePassService KeePassService { get; }
        public DownloadHandlerConfiguration Configuration { get; }
        public IComponentContext ComponentContext { get; }

        protected IWebDriver Browser;
        protected PwEntry KeePassEntry => KeePassService.GetEntryByUuid(Configuration.KeePassEntryUuid);
        protected ICollection<DownloadResult> DownloadResults;

        protected BankDownloadHandlerBase(IBankAccountRepository bankAccountRepository, DbContext dbContext, IKeePassService keePassService, DownloadHandlerConfiguration configuration, IComponentContext componentContext)
        {
            DbContext = dbContext;
            BankAccountRepository = bankAccountRepository;
            KeePassService = keePassService;
            Configuration = configuration;
            ComponentContext = componentContext;

            DownloadResults = new List<DownloadResult>();
        }

        public void Initialize(bool cleanupDirectoryBeforeStart)
        {
            Directory.CreateDirectory(Configuration.DownloadPath);
            if (cleanupDirectoryBeforeStart)
            {
                var archivePath = Path.Combine(Configuration.DownloadPath, "_old");
                Directory.CreateDirectory(archivePath);
                var datePrefix = DateTime.Now.ToSortableFileName();
                foreach (var file in Directory.GetFiles(Configuration.DownloadPath))
                {
                    File.Move(file,
                        Path.Combine(archivePath, $"{datePrefix}_{Path.GetFileName(file)}"));
                }

            }
            var options = new ChromeOptions();
            options.AddUserProfilePreference("download.default_directory", Configuration.DownloadPath);
            options.AddUserProfilePreference("profile.default_content_settings.popups", 0);

            Browser = new ChromeDriver(options);
            Browser.Manage().Window.Maximize();
            Browser.Navigate().GoToUrl(Configuration.WebSiteUrl);
        }

        public void Execute(bool cleanupDirectoryBeforeStart = false)
        {
            Initialize(cleanupDirectoryBeforeStart);
            Login();
            DownloadTransactions();
            NavigateHome();
            DownloadStatementsAndFiles();
            Logout();
            ProcessFiles();
        }

        protected void ProcessFiles()
        {
            foreach (var downloadResult in DownloadResults)
            {
                var repositoryType = typeof(IRepository<>).MakeGenericType(downloadResult.TargetEntity);
                var insertOrGetMethod = repositoryType.GetMethod("InsertOrGetWithEquality");

                var repository = ComponentContext.Resolve(repositoryType);
                var toInsert = downloadResult.FileParser.Parse(downloadResult.FilePath);
                foreach (var entity in toInsert)
                {
                    var persistedEntity = insertOrGetMethod.Invoke(repository, new[] { entity });
                    downloadResult.Account.Transactions.Add((BankTransactionEntity)persistedEntity);
                }
                DbContext.SaveChanges();
                //TODO check balance
            }
        }

        public void Dispose()
        {
            try
            {
                Browser.Dispose();
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        /// Starts from the login page (definied in configuration)
        /// Finishes an the home page.
        /// </summary>
        protected abstract void Login();
        /// <summary>
        /// Logs out and terminates session from any point on the page.
        /// </summary>
        protected abstract void Logout();
        /// <summary>
        /// Navigates to the home page.
        /// </summary>
        protected abstract void NavigateHome();
        /// <summary>
        /// Downloads all the transaction files.
        /// </summary>
        protected abstract void DownloadTransactions();
        /// <summary>
        /// Download all the availabe statements (pdf, ...) and other available files.
        /// </summary>
        protected abstract void DownloadStatementsAndFiles();

        public class DownloadResult
        {
            public string FilePath { get; set; }
            public Type TargetEntity { get; set; }
            public IFileParser FileParser { get; set; }
            public AccountEntity Account { get; set; }
        }
    }
}