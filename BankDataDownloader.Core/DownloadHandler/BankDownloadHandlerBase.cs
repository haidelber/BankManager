using System;
using System.IO;
using System.Security;
using BankDataDownloader.Common.Properties;
using BankDataDownloader.Core.DownloadHandler.Interfaces;
using BankDataDownloader.Core.Extension;
using BankDataDownloader.Core.KeePass;
using BankDataDownloader.Core.Selenium;
using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace BankDataDownloader.Core.DownloadHandler
{
    public abstract class BankDownloadHandlerBase : IBankDownloadHandler
    {
        public readonly Logger Log = LogManager.GetCurrentClassLogger();

        public string Url { get; }
        public string DownloadPath { get; }

        protected IWebDriver Browser;
        protected KeePassWrapper KeePass;
        protected SeleniumFileDownloader FileDownloader;

        private readonly string _keePassMasterPasswordString;
        private readonly SecureString _keePassMasterPasswordSecureString;

        private string KeePassMasterPassword => string.IsNullOrEmpty(_keePassMasterPasswordString)
            ? _keePassMasterPasswordSecureString.ConvertToUnsecureString()
            : _keePassMasterPasswordString;


        internal BankDownloadHandlerBase(string masterPassword, string url, string downloadPath)
        {
            Url = url;
            DownloadPath = downloadPath;

            _keePassMasterPasswordSecureString = null;
            _keePassMasterPasswordString = masterPassword;
        }

        internal BankDownloadHandlerBase(SecureString masterPassword, string url, string downloadPath)
        {
            Url = url;
            DownloadPath = downloadPath;

            _keePassMasterPasswordSecureString = masterPassword;
            _keePassMasterPasswordString = null;
        }

        public void Initialize()
        {
            if (!Directory.Exists(DownloadPath))
            {
                Directory.CreateDirectory(DownloadPath);
            }
            KeePass = KeePassWrapper.OpenWithPassword(SettingsHandler.Instance.KeePassPath, KeePassMasterPassword);

            var options = new ChromeOptions();
            options.AddUserProfilePreference("download.default_directory", DownloadPath);
            options.AddUserProfilePreference("profile.default_content_settings.popups", 0);

            Browser = new ChromeDriver(options);
            Browser.Manage().Window.Maximize();
            Browser.Navigate().GoToUrl(Url);

            FileDownloader = new SeleniumFileDownloader(Browser, DownloadPath);

            Login();
        }

        public void DownloadAllData()
        {
            Initialize();

            Download();
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

        public abstract void Login();
        public abstract void Logout();
        public abstract void NavigateHome();
        public abstract void Download();
    }
}