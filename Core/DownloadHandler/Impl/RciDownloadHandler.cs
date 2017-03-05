using System;
using System.Collections.Generic;
using System.IO;
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
using OpenQA.Selenium.Support.PageObjects;

namespace BankDataDownloader.Core.DownloadHandler.Impl
{
    public class RciDownloadHandler : BankDownloadHandlerBase
    {
        public RciDownloadHandler(IBankAccountRepository bankAccountRepository, IKeePassService keePassService, DownloadHandlerConfiguration configuration, IComponentContext componentContext) : base(bankAccountRepository, keePassService, configuration, componentContext)
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
                    ComponentContext.ResolveNamed<IValueParser>(Constants.UniqueContainerKeys.ValueParserGermanDecimal);
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
                BalanceSelectorFunc = () => BankAccountRepository.GetById(bankAccount.Id).Transactions.Sum(entity => entity.Amount),
                FileParser = ComponentContext.ResolveNamed<IFileParser>(Constants.UniqueContainerKeys.FileParserRci),
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