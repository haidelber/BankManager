﻿using System.Collections.Generic;
using System.Threading;
using Autofac;
using Autofac.Features.AttributeFilters;
using BankDataDownloader.Common;
using BankDataDownloader.Common.Model.Configuration;
using BankDataDownloader.Core.Extension;
using BankDataDownloader.Core.Model;
using BankDataDownloader.Core.Model.FileParser;
using BankDataDownloader.Core.Service;
using BankDataDownloader.Data.Entity;
using BankDataDownloader.Data.Repository;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace BankDataDownloader.Core.DownloadHandler.Impl
{
    public class SantanderDownloadHandler : BankDownloadHandlerBase
    {
        public SantanderDownloadHandler(IBankAccountRepository bankAccountRepository, IPortfolioRepository portfolioRepository, IPortfolioPositionRepository portfolioPositionRepository, IBankTransactionRepository bankTransactionRepository, IKeePassService keePassService, DownloadHandlerConfiguration configuration, IComponentContext componentContext) : base(bankAccountRepository, portfolioRepository, portfolioPositionRepository, bankTransactionRepository, keePassService, configuration, componentContext)
        {
        }

        protected override void Login()
        {
            using (KeePassService.Open())
            {
                Browser.FindElement(new ByChained(By.Id("eserviceLogin"), new ByIdOrName("disposerId")))
                    .SendKeys(KeePassEntry.GetUserName());
                Browser.FindElement(new ByChained(By.Id("eserviceLogin"), new ByIdOrName("birthdate")))
                    .SendKeys(KeePassEntry.GetString(Constants.DownloadHandler.SantanderBirthday));
                Browser.FindElement(new ByChained(By.Id("eserviceLogin"), new ByIdOrName("password")))
                    .SendKeys(KeePassEntry.GetPassword());

                Browser.FindElement(new ByChained(By.Id("eserviceLogin"), new ByIdOrName("submitButton"))).Click();
            }
        }

        protected override void Logout()
        {
            Browser.FindElement(new ByChained(By.Id("login"), By.TagName("a"))).Click();
        }

        protected override void NavigateHome()
        {
            Browser.FindElement(new ByChained(By.Id("header"), By.TagName("div"), By.TagName("a"))).Click();
        }

        protected override IEnumerable<FileParserInput> DownloadTransactions()
        {
            var downloadResults = new List<FileParserInput>();
            Browser.FindElement(new ByChained(By.Id("collapseTwo"), By.LinkText("Buchungen"))).Click();
            SetMaxDateRange();
            Browser.FindElement(By.Id("showPrint")).Submit();

            //TODO rewrite FileDownloader.DownloadCurrentPageSource("account.html", fileOtherPrefix: filePrefix);

            Browser.FindElement(By.Id("printBookings")).Submit();
            return downloadResults;
        }

        private void SetMaxDateRange()
        {
            var month = new SelectElement(Browser.FindElement(new ByChained(By.Id("showBookings"), new ByIdOrName("dateFrom_month"))));
            month.SelectByIndex(0);
            var year = new SelectElement(Browser.FindElement(new ByChained(By.Id("showBookings"), new ByIdOrName("dateFrom_year"))));
            year.SelectByIndex(0);
            Browser.FindElement(new ByChained(By.Id("showBookings"), By.TagName("table"), By.TagName("tbody"),
                By.TagName("tr"), By.XPath("td[5]"), By.TagName("input"))).Click();
        }

        protected override void DownloadStatementsAndFiles()
        {
            //Go to Nachrichten
            Browser.FindElement(By.XPath("//*[@id=\"main-menu\"]/li[2]/a")).Click();
            //Click on Messages until found correct message
            var foundLink = false;
            for (int i = 0; !foundLink; i++)
            {
                var selector = By.PartialLinkText("Kontoauszug BestFlex");
                var elements = Browser.FindElements(selector);
                elements[i].Click();
                try
                {
                    Browser.FindElement(By.LinkText("Kontauszug BestFlex")).Click();
                    foundLink = true;
                }
                catch (NoSuchElementException)
                {
                }
            }


            GetAccountSelect().SelectByIndex(1);
            //Start with idx 1 as first entry is empty
            for (int i = 1; i < GetYearSelect().Options.Count; i++)
            {
                GetYearSelect().SelectByIndex(i);
                //Start with idx 1 as first entry is empty
                for (int j = 1; j < GetMonthSelect().Options.Count; j++)
                {
                    GetAccountSelect().SelectByIndex(1);
                    //little waiting time to make JS execute
                    Thread.Sleep(50);
                    GetYearSelect().SelectByIndex(i);
                    Thread.Sleep(50);
                    GetMonthSelect().SelectByIndex(j);

                    //download button
                    Browser.FindElement(By.Id("eServiceForm")).Submit();
                    var downloadLink = Browser.FindElement(By.LinkText("Kontoauszug downloaden"));
                    downloadLink.Click();

                    //return to form
                    Browser.FindElement(By.XPath("//*[@id=\"esNext\"]/input")).Click();
                    GetAccountSelect().SelectByIndex(1);
                    Thread.Sleep(50);
                    GetYearSelect().SelectByIndex(i);
                    Thread.Sleep(50);
                    GetMonthSelect().SelectByIndex(j);
                }
            }
        }

        private SelectElement GetAccountSelect()
        {
            return new SelectElement(Browser.FindElement(By.Id("applyTo")));
        }

        private SelectElement GetYearSelect()
        {
            return new SelectElement(Browser.FindElement(By.Id("years_year")));
        }

        private SelectElement GetMonthSelect()
        {
            return new SelectElement(Browser.FindElement(By.Id("months_month")));
        }
    }
}