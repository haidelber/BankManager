using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autofac;
using BankDataDownloader.Common.Extensions;
using BankDataDownloader.Common.Helper;
using BankDataDownloader.Common.Model.Configuration;
using BankDataDownloader.Core.Extension;
using BankDataDownloader.Core.Model;
using BankDataDownloader.Core.Service;
using BankDataDownloader.Data.Repository;
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
        public IKeePassService KeePassService { get; }
        public DownloadHandlerConfiguration Configuration { get; }
        public IComponentContext ComponentContext { get; }

        protected IWebDriver Browser;
        protected PwEntry KeePassEntry => KeePassService.GetEntryByUuid(Configuration.KeePassEntryUuid);

        protected BankDownloadHandlerBase(IBankAccountRepository bankAccountRepository, IKeePassService keePassService, DownloadHandlerConfiguration configuration, IComponentContext componentContext)
        {
            BankAccountRepository = bankAccountRepository;
            KeePassService = keePassService;
            Configuration = configuration;
            ComponentContext = componentContext;
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
            //Old Chrome pre 57 version to disable internal pdf viewer
            //options.AddUserProfilePreference("plugins.plugins_disabled", new[]
            //{
            //    "Chrome PDF Viewer"
            //});
            //Chrome 57+ variant
            options.AddUserProfilePreference("plugins.always_open_pdf_externally", true);
            //TODO make headless as soon as it's available http://stackoverflow.com/a/34170686/4759472
            //options.AddArguments("--headless", "--disable-gpu", "--remote-debugging-port=9222");

            Browser = new ChromeDriver(options);
            Browser.Manage().Window.Maximize();
            Browser.Navigate().GoToUrl(Configuration.WebSiteUrl);
        }

        public void Execute(bool cleanupDirectoryBeforeStart = false)
        {
            Initialize(cleanupDirectoryBeforeStart);
            Login();
            //ToList forces the execution of DownloadTransactions before next statement
            var filesToParse = DownloadTransactions().ToList();
            NavigateHome();
            DownloadStatementsAndFiles();
            Logout();
            ProcessFiles(filesToParse);
        }

        public void ProcessFiles(IEnumerable<FileParserInput> filesToParse)
        {
            foreach (var downloadResult in filesToParse)
            {
                var repositoryType = typeof(IRepository<>).MakeGenericType(downloadResult.TargetEntity);
                var insertOrGetMethod = repositoryType.GetMethod("InsertOrGetWithEquality");
                var saveMethod = repositoryType.GetMethod("Save");
                var accountProperty =
                    downloadResult.TargetEntity.GetProperties()
                        .Single(info => info.PropertyType.IsInstanceOfType(downloadResult.OwningEntity));

                var repository = ComponentContext.Resolve(repositoryType);
                var toInsert = downloadResult.FileParser.Parse(downloadResult.FilePath);
                foreach (var entity in toInsert)
                {
                    accountProperty.SetValue(entity, downloadResult.OwningEntity);
                    var persistedEntity = insertOrGetMethod.Invoke(repository, new[] { entity });

                    //downloadResult.Account.Transactions.Add((BankTransactionEntity)persistedEntity);
                }
                saveMethod.Invoke(repository, null);
                //TODO some kind of unit of work autocommit
                if (downloadResult.BalanceSelectorFunc != null)
                {
                    if (downloadResult.BalanceSelectorFunc() != downloadResult.Balance)
                    {
                        throw new InvalidOperationException("Balance check failed");
                    }
                }
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

        protected string DownloadFromWebElement(IWebElement element, string fileName)
        {
            var filesPreDownload = Directory.GetFiles(Configuration.DownloadPath);

            element.Click();

            Browser.WaitForDownloadToFinishByDirectory(Configuration.DownloadPath, filesPreDownload.Length + 1);
            var file = Directory.GetFiles(Configuration.DownloadPath).Single(s => !filesPreDownload.Contains(s));
            return Helper.FileRename(file, fileName);
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
        protected abstract IEnumerable<FileParserInput> DownloadTransactions();
        /// <summary>
        /// Download all the availabe statements (pdf, ...) and other available files.
        /// </summary>
        protected abstract void DownloadStatementsAndFiles();

        protected void TakeScreenshot(string fileName)
        {
            Screenshot ss = ((ITakesScreenshot)Browser).GetScreenshot();
#pragma warning disable 618
            ss.SaveAsFile(Path.Combine(Configuration.DownloadPath, $"{fileName}.png"), System.Drawing.Imaging.ImageFormat.Png);
#pragma warning restore 618
        }
    }
}