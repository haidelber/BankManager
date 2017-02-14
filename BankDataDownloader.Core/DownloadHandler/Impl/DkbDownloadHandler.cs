using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Autofac;
using BankDataDownloader.Common.Model.Configuration;
using BankDataDownloader.Core.Extension;
using BankDataDownloader.Core.Model;
using BankDataDownloader.Core.Service;
using BankDataDownloader.Core.Service.Impl;
using BankDataDownloader.Data.Repository;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace BankDataDownloader.Core.DownloadHandler.Impl
{
    public class DkbDownloadHandler : BankDownloadHandlerBase
    {
        public DkbDownloadHandler(IBankAccountRepository bankAccountRepository, IKeePassService keePassService, DownloadHandlerConfiguration configuration, IComponentContext componentContext) : base(bankAccountRepository, keePassService, configuration, componentContext)
        {
        }

        protected override void Login()
        {
            Browser.FindElement(By.Id("loginInputSelector")).SendKeys(KeePassEntry.GetUserName());
            Browser.FindElement(By.Id("pinInputSelector")).SendKeys(KeePassEntry.GetPassword());

            Browser.FindElement(By.Id("login")).Submit();
        }

        protected override void Logout()
        {
            Browser.FindElement(By.Id("logout")).Click();
        }

        protected override void NavigateHome()
        {
            Browser.FindElement(By.ClassName("dkb_logo_container")).Click();
        }

        protected override IEnumerable<FileParserInput> DownloadTransactions()
        {
            var downloadResults = new List<FileParserInput>();
            //bankaccount
            NavigateHome();
            GetAccountTransactions()[0].Click();
            SetMaxDateRange("[id*=transactionDate]", "[id*=toTransactionDate]");
            Browser.FindElement(By.ClassName("evt-csvExport")).Click();

            //credit card
            NavigateHome();
            GetAccountTransactions()[1].Click();
            SetMaxDateRange("[id*=postingDate]", "[id*=toPostingDate]");
            Browser.FindElement(By.ClassName("evt-csvExport")).Click();
            return downloadResults;
        }

        protected override void DownloadStatementsAndFiles()
        {
            NavigateHome();

            GetPostboxMenuItem().Click();
            for (var i = 0; i < GetSubPostboxMenuItems().Count; i++)
            {
                GetSubPostboxMenuItems()[i].Click();

                foreach (var fileLink in Browser.FindElements(By.ClassName("iconSpeichern0")))
                {
                    fileLink.Click();
                }
            }
        }

        private List<IWebElement> GetAccountTransactions()
        {
            return Browser.FindElements(new ByChained(By.ClassName("financialStatusTable"), By.ClassName("mainRow"), By.ClassName("evt-paymentTransaction"))).ToList();
        }

        private void SetMaxDateRange(string cssSelectorFromDate, string cssSelectorToDate)
        {
            var dateFormatString = "dd.MM.yyyy";

            var startRange = Browser.FindElement(new ByAll(By.TagName("input"), By.CssSelector(cssSelectorFromDate)));
            startRange.Clear();
            startRange.SendKeys(DateTime.Today.AddYears(-5).ToString(dateFormatString));

            var endRange = Browser.FindElement(new ByAll(By.TagName("input"), By.CssSelector(cssSelectorToDate)));
            endRange.Clear();
            endRange.SendKeys(DateTime.Today.ToString(dateFormatString));

            Browser.FindElement(By.Id("searchbutton")).Click();
            //Date has been adapted now click again
            Browser.FindElement(By.Id("searchbutton")).Click();
        }

        private IWebElement GetPostboxMenuItem()
        {
            return Browser.FindElement(new ByChained(By.Id("menu"), By.LinkText("Postfach")));
        }

        private List<IWebElement> GetSubPostboxMenuItems()
        {
            return GetPostboxMenuItem().FindElements(By.XPath("..//ul//a")).ToList();
        }
    }
}