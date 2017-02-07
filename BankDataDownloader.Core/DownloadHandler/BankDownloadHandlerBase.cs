using System;
using System.IO;
using BankDataDownloader.Common.Model.Configuration;
using BankDataDownloader.Core.Service.Impl;
using KeePassLib;
using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace BankDataDownloader.Core.DownloadHandler
{
    public abstract class BankDownloadHandlerBase : IBankDownloadHandler
    {
        public readonly Logger Log = LogManager.GetCurrentClassLogger();

        public KeePassService KeePassService { get; set; }
        public DownloadHandlerConfiguration Configuration { get; set; }

        protected IWebDriver Browser;
        protected PwEntry KeePassEntry => KeePassService.GetEntryByUuid(Configuration.KeePassEntryUuid); 

        protected BankDownloadHandlerBase(KeePassService keePassService, DownloadHandlerConfiguration configuration)
        {
            KeePassService = keePassService;
            Configuration = configuration;
        }

        public void Initialize(bool cleanupDirectoryBeforeStart)
        {
            Directory.CreateDirectory(Configuration.DownloadPath);
            if (cleanupDirectoryBeforeStart)
            {
                var datePrefix = DateTime.Now.ToString("s");
                foreach (var file in Directory.GetFiles(Configuration.DownloadPath))
                {
                    File.Move(file,
                        Path.Combine(Configuration.DownloadPath, "_old", $"{datePrefix}_{Path.GetFileName(file)}"));
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
        }

        public void Dispose()
        {
            try
            {
                Logout();

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
    }
}