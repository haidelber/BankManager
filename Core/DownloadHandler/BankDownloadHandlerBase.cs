using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autofac;
using BankManager.Common.Exceptions;
using BankManager.Common.Extensions;
using BankManager.Common.Model.Configuration;
using BankManager.Core.Extension;
using BankManager.Core.Model.FileParser;
using BankManager.Core.Service;
using BankManager.Data.Entity;
using BankManager.Data.Repository;
using KeePassLib;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace BankManager.Core.DownloadHandler
{
    public abstract class BankDownloadHandlerBase : IBankDownloadHandler
    {
        public ILogger Logger { get; }
        public IBankAccountRepository BankAccountRepository { get; }
        public IPortfolioRepository PortfolioRepository { get; }
        public IPortfolioPositionRepository PortfolioPositionRepository { get; }
        public IBankTransactionRepository<TransactionEntity> BankTransactionRepository { get; }

        public IKeePassService KeePassService { get; }
        public IImportService ImportService { get; }
        public DownloadHandlerConfiguration Configuration { get; }
        public IComponentContext ComponentContext { get; }

        protected IWebDriver Browser;
        protected PwEntry KeePassEntry => KeePassService.GetEntryByUuid(Configuration.KeePassEntryUuid);

        protected BankDownloadHandlerBase(ILogger logger, IBankAccountRepository bankAccountRepository, IPortfolioRepository portfolioRepository, IPortfolioPositionRepository portfolioPositionRepository, IBankTransactionRepository<TransactionEntity> bankTransactionRepository, IKeePassService keePassService, DownloadHandlerConfiguration configuration, IComponentContext componentContext, IImportService importService)
        {
            Logger = logger;
            BankAccountRepository = bankAccountRepository;
            PortfolioRepository = portfolioRepository;
            PortfolioPositionRepository = portfolioPositionRepository;
            BankTransactionRepository = bankTransactionRepository;
            KeePassService = keePassService;
            Configuration = configuration;
            ComponentContext = componentContext;
            ImportService = importService;
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

        public void Execute(bool cleanupDirectoryBeforeStart = false, bool downloadStatements = true)
        {
            Initialize(cleanupDirectoryBeforeStart);
            Login();
            //consider delayed execution because of yield
            var filesToParse = DownloadTransactions();
            ProcessFiles(filesToParse);
            NavigateHome();
            if (downloadStatements)
            {
                DownloadStatementsAndFiles();
            }
            Logout();
        }

        public void ProcessFiles(IEnumerable<FileParserInput> filesToParse)
        {
            foreach (var downloadResult in filesToParse)
            {
                ImportService.Import(downloadResult);
                //TODO some kind of unit of work autocommit
                if (downloadResult.BalanceSelectorFunc != null)
                {
                    //TODO is this fixed now with proper EF implementation? Unfortunately Sqlite doesn't handle decimal, so we have to deal with double comparison issues
                    var actualBalance = Math.Round(downloadResult.BalanceSelectorFunc(), 2);
                    if (Math.Abs(actualBalance - downloadResult.Balance) >= 0.01m)
                    {
                        throw new BalanceCheckException(downloadResult.Balance, actualBalance, "Balance check failed");
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
            return Common.Helper.Helper.FileRename(file, fileName);
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
            ss.SaveAsFile(Path.Combine(Configuration.DownloadPath, $"{fileName}.png"), ScreenshotImageFormat.Png);
#pragma warning restore 618
        }
    }
}