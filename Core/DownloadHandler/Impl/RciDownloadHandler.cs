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
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace BankManager.Core.DownloadHandler.Impl
{
    public class RciDownloadHandler : BankDownloadHandlerBase
    {
        public RciDownloadHandler(IBankAccountRepository bankAccountRepository, IPortfolioRepository portfolioRepository, IPortfolioPositionRepository portfolioPositionRepository, IBankTransactionRepository<TransactionEntity> bankTransactionRepository, IKeePassService keePassService, DownloadHandlerConfiguration configuration, IComponentContext componentContext, IImportService importService) : base(bankAccountRepository, portfolioRepository, portfolioPositionRepository, bankTransactionRepository, keePassService, configuration, componentContext, importService)
        {
        }

        protected override void Login()
        {
            using (KeePassService.Open())
            {
                Browser.FindElement(By.Id("username")).SendKeys(KeePassEntry.GetUserName());
                Browser.FindElement(new ByChained(By.Id("login"), By.XPath("//input[@type='password']")))
                    .SendKeys(KeePassEntry.GetPassword());
                Browser.FindElement(By.Id("submitButton")).Click();
            }
        }

        protected override void Logout()
        {
            Browser.FindElement(By.ClassName("kontoLogout")).Click();
        }

        protected override void NavigateHome()
        {
            //Browser.FindElement(By.XPath("//*[@id='mainMenu']/ul/li[1]/a")).Click();
            Browser.FindElement(By.PartialLinkText("KONTEN & ZAHLUNGSVERKEHR")).Click();
        }

        protected override IEnumerable<FileParserInput> DownloadTransactions()
        {
            Browser.WaitForJavaScript(5000);
            Browser.FindElement(By.XPath("//*[@id='subSubMenu']/li[2]/a")).Click();
            //Browser.FindElement(By.PartialLinkText("Kontoübersicht")).Click();

            var accountNumber = Browser.FindElements(By.ClassName("contenttypo"))[1].FindElement(By.TagName("span")).Text.CleanString();
            var iban = Browser.FindElements(By.ClassName("contenttypo"))[3].FindElement(By.TagName("span")).Text.CleanString();
            var balanceStr = Browser.FindElements(By.ClassName("contenttypo"))[9].FindElement(By.TagName("span")).Text.CleanNumberStringFromOther();
            var valueParser =
                    ComponentContext.ResolveKeyed<IValueParser>(Constants.UniqueContainerKeys.ValueParserGermanDecimal);
            var balance = (decimal)valueParser.Parse(balanceStr);

            var bankAccount = BankAccountRepository.GetByIban(iban);
            if (bankAccount == null)
            {
                bankAccount = new BankAccountEntity
                {
                    AccountNumber = accountNumber,
                    Iban = iban,
                    BankName = Constants.DownloadHandler.BankNameRci,
                    AccountName = Constants.DownloadHandler.AccountNameSaving
                };
                BankAccountRepository.Insert(bankAccount);
            }

            TakeScreenshot(iban);

            //*[@id="submitButton"]
            Browser.FindElement(By.Name("trigger:BUTTON2::")).Click();
            //From date input
            var fromDate = Browser.FindElement(new ByChained(By.CssSelector(".field.cf.west"), By.TagName("input")));
            fromDate.Clear();
            fromDate.SendKeys(DateTime.Today.AddYears(-1).ToString("dd.MM.yyyy"));
            //Checkbox details
            var checkboxDivs = Browser.FindElements(By.CssSelector(".krcheck.checkbox"));
            var detailsCheckbox = checkboxDivs.Last();
            if (!detailsCheckbox.Selected)
            {
                detailsCheckbox.Click();
            }
            //Submit
            Browser.FindElement(By.Id("default")).Click();

            //Download
            var file = DownloadFromWebElement(Browser.FindElement(By.XPath("//a[@title='Download']")), iban);

            var originalHandle = Browser.CurrentWindowHandle;
            foreach (var windowHandle in Browser.WindowHandles)
            {
                if (!windowHandle.Equals(originalHandle))
                {
                    Browser.SwitchTo().Window(windowHandle);
                    Browser.Close();
                }
            }
            Browser.SwitchTo().Window(originalHandle);
            yield return new FileParserInput
            {
                Balance = balance,
                BalanceSelectorFunc = () => BankTransactionRepository.TransactionSumForAccountId(bankAccount.Id),
                FileParser = ComponentContext.ResolveKeyed<IFileParser>(Constants.UniqueContainerKeys.FileParserRci),
                FilePath = file,
                OwningEntity = bankAccount,
                TargetEntity = typeof(RciTransactionEntity)
            };
        }

        protected override void DownloadStatementsAndFiles()
        {
            Browser.WaitForJavaScript(5000);
            Browser.FindElement(By.PartialLinkText("POSTFACH")).Click();
            Browser.FindElement(By.Name("trigger:postfachbutton::")).Click();

            //switch tab
            var originalHandle = Browser.CurrentWindowHandle;
            string postfachHandle = null;
            foreach (var windowHandle in Browser.WindowHandles)
            {
                if (!windowHandle.Equals(originalHandle))
                {
                    Browser.SwitchTo().Window(windowHandle);
                    if (Browser.Title.Equals("Postfach", StringComparison.OrdinalIgnoreCase))
                    {
                        postfachHandle = windowHandle;
                    }
                    else
                    {
                        Browser.Close();
                    }
                }
            }
            Browser.SwitchTo().Window(postfachHandle);

            var byDate = new ByChained(By.ClassName("inboxCol2"), By.TagName("a"));
            while (Browser.FindElements(byDate).Count > 0)
            {
                var dateLink = Browser.FindElement(byDate);
                var dateText = dateLink.Text;
                var date = DateTime.Parse(dateText);

                dateLink.Click();

                var fileName = Browser.FindElement(By.ClassName("detailCol1")).Text;

                DownloadFromWebElement(Browser.FindElement(By.ClassName("btnDownload")), date.ToString("yyyy-MM"));

                Browser.FindElement(By.XPath("//*[@id='deliveryActions']/input[@value='LÖSCHEN']")).Click();
                Browser.FindElement(By.Id("ja")).Click();
            }
            Browser.Close();
            Browser.SwitchTo().Window(originalHandle);
        }
    }
}