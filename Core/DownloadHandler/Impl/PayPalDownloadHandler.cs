﻿using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using BankDataDownloader.Common;
using BankDataDownloader.Common.Extensions;
using BankDataDownloader.Common.Model.Configuration;
using BankDataDownloader.Core.Extension;
using BankDataDownloader.Core.Model;
using BankDataDownloader.Core.Parser;
using BankDataDownloader.Core.Service;
using BankDataDownloader.Data.Entity;
using BankDataDownloader.Data.Entity.BankTransactions;
using BankDataDownloader.Data.Repository;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace BankDataDownloader.Core.DownloadHandler.Impl
{
    public class PayPalDownloadHandler : BankDownloadHandlerBase
    {
        public IAccountRepository AccountRepository { get; }
        public PayPalDownloadHandler(IBankAccountRepository bankAccountRepository, IKeePassService keePassService, DownloadHandlerConfiguration configuration, IComponentContext componentContext, IAccountRepository accountRepository) : base(bankAccountRepository, keePassService, configuration, componentContext)
        {
            AccountRepository = accountRepository;
        }

        protected override void Login()
        {
            using (KeePassService.Open())
            {
                Browser.FindElement(By.Name("login_email")).SendKeys(KeePassEntry.GetUserName());
                Browser.FindElement(By.Name("login_password")).SendKeys(KeePassEntry.GetPassword());
                Browser.FindElement(By.Name("btnLogin")).Click();
            }
        }

        protected override void Logout()
        {
            try
            {
                Browser.FindElement(By.ClassName("vx_globalNav-link_logout")).Click();
            }
            catch (NoSuchElementException)
            {
            }
            try
            {
                Browser.FindElement(By.ClassName("logout")).Click();
            }
            catch (NoSuchElementException)
            {
            }
        }

        protected override void NavigateHome()
        {
            try
            {
                Browser.FindElement(By.ClassName("vx_globalNav-brand_desktop")).Click();
            }
            catch (NoSuchElementException)
            {
            }
            try
            {
                Browser.FindElement(By.ClassName("logo")).Click();
            }
            catch (NoSuchElementException)
            {
            }
        }

        protected override IEnumerable<FileParserInput> DownloadTransactions()
        {
            var valueParser =
                    ComponentContext.ResolveNamed<IValueParser>(Constants.UniqueContainerKeys.ValueParserGermanDecimal);

            var account =
                AccountRepository.GetByAccountNameAndBankName(Constants.DownloadHandler.AccountNamePaymentService,
                    Constants.DownloadHandler.BankNamePayPal);
            if (account == null)
            {
                account = new AccountEntity
                {
                    BankName = Constants.DownloadHandler.BankNamePayPal,
                    AccountName = Constants.DownloadHandler.AccountNamePaymentService
                };
                AccountRepository.Insert(account);
            }

            var balanceEntries =
                Browser.FindElements(By.ClassName("currenciesEntry"))
                    .Select(element => (decimal)valueParser.Parse(element.Text.CleanNumberStringFromOther()));
            var balance = balanceEntries.FirstOrDefault();

            Browser.WaitForJavaScript(5000);

            TakeScreenshot("screenshot");

            Browser.Navigate().GoToUrl("https://www.paypal.com/cgi-bin/webscr?cmd=_history-download");

            //set date
            Browser.FindElement(By.Name("from_a")).Clear();
            Browser.FindElement(By.Name("from_b")).Clear();
            Browser.FindElement(By.Name("from_c")).Clear();
            var startDate = DateTime.Now.AddYears(-2).AddDays(1);
            Browser.FindElement(By.Name("from_a")).SendKeys(startDate.Month.ToString()); //Month
            Browser.FindElement(By.Name("from_b")).SendKeys(startDate.Day.ToString()); //Day
            Browser.FindElement(By.Name("from_c")).SendKeys(startDate.Year.ToString()); //Year

            var downloadType = new SelectElement(Browser.FindElement(By.Name("custom_file_type")));
            downloadType.SelectByValue("comma_balaffecting");

            var resultingFile = DownloadFromWebElement(Browser.FindElement(By.Name("submit.x")), "transactions");

             yield return new FileParserInput
            {
                OwningEntity = account,
                FileParser =
                    ComponentContext.ResolveNamed<IFileParser>(Constants.UniqueContainerKeys.FileParserPayPal),
                FilePath = resultingFile,
                TargetEntity = typeof(PayPalTransactionEntity),
                Balance = balance,
                BalanceSelectorFunc =
                    () => AccountRepository.GetById(account.Id).Transactions.Sum(entity => entity.Amount)
            };
        }

        protected override void DownloadStatementsAndFiles()
        {
            //nothing to do here
        }
    }
}