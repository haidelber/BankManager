using DataDownloader.Properties;
using KeePass;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;

namespace DataDownloader
{
    public abstract class BankDownloaderBase
    {
        protected IWebDriver Browser;
        protected KeePassWrapper KeePass;
        protected static string Url;

        [TestInitialize]
        public void TestInitialize()
        {
            KeePass = KeePassWrapper.OpenWithPassword(Settings.Default.KeePass_Path, Settings.Default.KeePass_MasterPassword);
            Browser = new ChromeDriver(@"webdriver");
            //Browser = new FirefoxDriver();
            //Browser = new InternetExplorerDriver();
            Browser.Navigate().GoToUrl(Url);

            Login();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Logout();

            Browser.Dispose();
        }

        protected abstract void Login();
        protected abstract void Logout();
    }
}