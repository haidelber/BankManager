﻿using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using BankManager.Common;
using BankManager.Common.Extensions;
using BankManager.Common.Model.Configuration;
using BankManager.Core.Extension;
using BankManager.Core.Model.FileParser;
using BankManager.Core.Parser;
using BankManager.Core.Service;
using BankManager.Data.Entity;
using BankManager.Data.Entity.BankTransactions;
using BankManager.Data.Repository;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace BankManager.Core.DownloadHandler.Impl
{
    public class PayPalDownloadHandler : BankDownloadHandlerBase
    {
        public PayPalDownloadHandler(ILogger<PayPalDownloadHandler> logger, IBankAccountRepository bankAccountRepository, IPortfolioRepository portfolioRepository, IPortfolioPositionRepository portfolioPositionRepository, IBankTransactionRepository<TransactionEntity> bankTransactionRepository, IKeePassService keePassService, DownloadHandlerConfiguration configuration, IComponentContext componentContext, IImportService importService) : base(logger, bankAccountRepository, portfolioRepository, portfolioPositionRepository, bankTransactionRepository, keePassService, configuration, componentContext, importService)
        {
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
                    ComponentContext.ResolveKeyed<IValueParser>(Constants.UniqueContainerKeys.ValueParserGermanDecimal);

            var account =
                BankAccountRepository.GetByAccountNameAndBankName(Constants.DownloadHandler.AccountNamePaymentService,
                    Constants.DownloadHandler.BankNamePayPal);
            if (account == null)
            {
                account = new BankAccountEntity
                {
                    BankName = Constants.DownloadHandler.BankNamePayPal,
                    AccountName = Constants.DownloadHandler.AccountNamePaymentService
                };
                BankAccountRepository.Insert(account);
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
                   ComponentContext.ResolveKeyed<IFileParser>(Constants.UniqueContainerKeys.FileParserPayPal),
                FilePath = resultingFile,
                TargetEntity = typeof(PayPalTransactionEntity),
                Balance = balance,
                BalanceSelectorFunc =
                   () => BankTransactionRepository.TransactionSumForAccountId(account.Id)
            };
        }

        protected override void DownloadStatementsAndFiles()
        {
            //nothing to do here
        }
    }
}