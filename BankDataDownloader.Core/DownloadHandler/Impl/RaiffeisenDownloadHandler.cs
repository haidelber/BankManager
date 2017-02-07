using System.Collections.Generic;
using System.IO;
using System.Linq;
using BankDataDownloader.Common;
using BankDataDownloader.Common.Model.Configuration;
using BankDataDownloader.Core.Extension;
using BankDataDownloader.Core.Service.Impl;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace BankDataDownloader.Core.DownloadHandler.Impl
{

    public class RaiffeisenDownloadHandler : BankDownloadHandlerBase
    {
        public RaiffeisenDownloadHandler(KeePassService keePassService, DownloadHandlerConfiguration configuration) : base(keePassService, configuration)
        {
        }

        protected override void Login()
        {
            //change to username login
            Browser.FindElement(By.Id("tab-benutzer")).Click();

            //type username
            Browser.FindElement(new ByIdOrName("loginform:LOGINNAME")).SendKeys(KeePassEntry.GetUserName());

            //type password
            Browser.FindElement(new ByIdOrName("loginform:LOGINPASSWD")).SendKeys(KeePassEntry.GetPassword());

            //check pass
            Browser.FindElement(new ByIdOrName("loginform:checkPasswort")).Click();

            //type pin
            Browser.FindElement(new ByIdOrName("loginpinform:PIN")).SendKeys(KeePassEntry.GetString(Constants.DownloadHandler.RaiffeisenPin));

            //final login
            Browser.FindElement(new ByIdOrName("loginpinform:anmeldenPIN")).Click();
        }

        protected override void Logout()
        {
            Browser.FindElement(new ByAll(By.ClassName("button"), By.ClassName("logoutlink"))).Click();
        }

        protected override void NavigateHome()
        {
            Browser.FindElement(new ByChained(By.Id("nav"), By.TagName("ul"), By.TagName("li"), By.TagName("a"))).Click();
        }

        protected override void DownloadTransactions()
        {
            for (int i = 0; i < GetAccountLinks().Count; i++)
            {
                var accountNumber = $"konto_{GetAccountLinks()[i].Text}";
                GetAccountLinks()[i].Click();

                Screenshot ss = ((ITakesScreenshot)Browser).GetScreenshot();
                ss.SaveAsFile(Path.Combine(Configuration.DownloadPath, $"{accountNumber}.png"), System.Drawing.Imaging.ImageFormat.Png);

                SetMaxDateRange();

                Browser.FindElement(
                new ByChained(By.ClassName("serviceButtonArea"),
                    new ByAll(By.ClassName("formControlButton"), By.ClassName("print")))).Click();

                DownloadCsv();
                NavigateHome();
            }
            DownloadDepots();
        }

        protected override void DownloadStatementsAndFiles()
        {
            //nothing
        }

        private void NavigateDepots()
        {
            NavigateHome();
            Browser.FindElement(By.LinkText("Depots")).Click();
        }

        private void DownloadDepots()
        {
            try
            {
                NavigateDepots();

                for (int i = 0; i < GetAccountLinks().Count; i++)
                {
                    var accountNumber = $"depot_{GetAccountLinks()[i].Text.Split('/')[1].Trim()}";
                    GetAccountLinks()[i].Click();

                    Browser.FindElement(new ByChained(By.ClassName("serviceButtonArea"), By.LinkText("Daten exportieren"))).Click();

                    DownloadCsv();

                    NavigateDepots();
                }
            }
            catch (NoSuchElementException)
            {
            }
        }

        private void DownloadCsv()
        {
            var combo =
                new SelectElement(Browser.FindElement(new ByChained(By.ClassName("mainInput"), By.TagName("select"))));
            combo.SelectByValue("CSV");

            Browser.FindElement(By.LinkText("Datei erstellen")).Click();
        }

        private void SetMaxDateRange()
        {
            Browser.FindElement(By.Id("kontoauswahlSelectionToggleLink")).Click();

            var month = new SelectElement(Browser.FindElement(By.ClassName("cal-month-year")));
            month.SelectByIndex(0);

            var day = new SelectElement(Browser.FindElement(By.ClassName("cal-day")));
            day.SelectByIndex(0);

            Browser.FindElement(new ByChained(By.ClassName("boxFormFooter"),
                new ByAll(By.ClassName("button"), By.ClassName("button-colored")))).Click();
        }

        private List<IWebElement> GetAccountLinks()
        {
            return Browser.FindElements(
                new ByChained(
                    By.ClassName("kontoTable"),
                    By.TagName("tbody"),
                    By.TagName("tr"),
                    By.XPath("td[1]"),
                    By.TagName("a")
                    )).ToList();
        }
    }
}
