using System.Collections.Generic;
using System.IO;
using System.Threading;
using BankDataDownloader.Core.Selenium;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.Extensions;

namespace BankDataDownloader.Core.Extension
{
    public static class SeleniumExtensions
    {
        public static By Or(this By by, By otherBy)
        {
            return new ByAllDisjunctive(by, otherBy);
        }

        public static IWebElement GetParent(this IWebElement node)
        {
            return node.FindElement(By.XPath(".."));
        }

        public static IWebElement FindElementOnPage(this IWebDriver webDriver, By by)
        {
            RemoteWebElement element = (RemoteWebElement)webDriver.FindElement(by);
            var hack = element.LocationOnScreenOnceScrolledIntoView;
            return element;
        }

        public static void WaitForJavaScript(this IWebDriver webDriver, int timeout = 1500)
        {
            //var wait = new WebDriverWait(webDriver, new TimeSpan(1000));
            //wait.Until(driver => driver.ExecuteJavaScript<string>("return document.readyState").Equals("complete"));
            Thread.Sleep(timeout);
        }

        public static void WaitForDownloadToFinishByDirectory(this IWebDriver webDriver, string directoryPath, int filesExpeced = -1, int timeout = 250)
        {
            while (filesExpeced != -1 && Directory.GetFiles(directoryPath).Length != filesExpeced)
            {
                Thread.Sleep(timeout);
            }
            while (Directory.GetFiles(directoryPath, "*.crdownload", SearchOption.TopDirectoryOnly).Length > 0 || Directory.GetFiles(directoryPath, "*.tmp", SearchOption.TopDirectoryOnly).Length > 0)
            {
                Thread.Sleep(timeout);
            }
        }

        public static void WaitForDownloadToFinishByFile(this IWebDriver webDriver, string expectedFilePath)
        {
            File.Exists(expectedFilePath);
        }

        public static Dictionary<string, object> GetAllAttributes(this IWebDriver webDriver, IWebElement webElement)
        {
            return webDriver.ExecuteJavaScript<Dictionary<string, object>>(
                "var items = {}; for (index = 0; index < arguments[0].attributes.length; ++index) { items[arguments[0].attributes[index].name] = arguments[0].attributes[index].value }; return items;",
                webElement);
        }
    }
}