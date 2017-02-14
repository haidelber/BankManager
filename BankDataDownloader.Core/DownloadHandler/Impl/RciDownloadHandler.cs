using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
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
    public class RciDownloadHandler : BankDownloadHandlerBase
    {
        public RciDownloadHandler(IBankAccountRepository bankAccountRepository, IKeePassService keePassService, DownloadHandlerConfiguration configuration, IComponentContext componentContext) : base(bankAccountRepository, keePassService, configuration, componentContext)
        {
        }

        protected override void Login()
        {
            Browser.FindElement(By.Id("username")).SendKeys(KeePassEntry.GetUserName());
            Browser.FindElement(new ByChained(By.Id("login"), By.XPath("//input[@type='password']")))
                .SendKeys(KeePassEntry.GetPassword());
            Browser.FindElement(By.Id("submitButton")).Click();
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
            var downloadResults = new List<FileParserInput>();
            Browser.WaitForJavaScript(5000);
            NavigateHome();
            Browser.FindElement(By.XPath("//*[@id='subSubMenu']/li[2]/a")).Click();
            //Browser.FindElement(By.PartialLinkText("Kontoübersicht")).Click();

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

            //Screenshot
            Screenshot ss = ((ITakesScreenshot)Browser).GetScreenshot();
            ss.SaveAsFile(Path.Combine(Configuration.DownloadPath, "screenshot.png"), System.Drawing.Imaging.ImageFormat.Png);

            //Download
            Browser.FindElement(By.XPath("//a[@title='Download']")).Click();

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
            return downloadResults;
        }

        protected override void DownloadStatementsAndFiles()
        {
            Browser.WaitForJavaScript(5000);
            NavigateHome();
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

            foreach (var file in Directory.GetFiles(Configuration.DownloadPath, "*).pdf"))
            {
                File.Delete(file);
            }

            var byDate = new ByChained(By.ClassName("inboxCol2"), By.TagName("a"));
            while (Browser.FindElements(byDate).Count > 0)
            {
                var dateLink = Browser.FindElement(byDate);
                var dateText = dateLink.Text;
                var date = DateTime.Parse(dateText);
                dateLink.Click();

                var fileName = Browser.FindElement(By.ClassName("detailCol1")).Text;
                Browser.FindElement(By.ClassName("btnDownload")).Click();

                //rename file
                var origPath = Path.Combine(Configuration.DownloadPath, fileName);
                File.Move(origPath, Path.Combine(Configuration.DownloadPath, date.ToString("yyyy-MM") + Path.GetExtension(origPath)));

                Browser.FindElement(By.XPath("//*[@id='deliveryActions']/input[@value='LÖSCHEN']")).Click();
                Browser.FindElement(By.Id("ja")).Click();
            }
            Browser.Close();
            Browser.SwitchTo().Window(originalHandle);
        }
    }
}