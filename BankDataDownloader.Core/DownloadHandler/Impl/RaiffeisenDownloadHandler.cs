using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using Autofac;
using BankDataDownloader.Common;
using BankDataDownloader.Common.Extensions;
using BankDataDownloader.Common.Helper;
using BankDataDownloader.Common.Model.Configuration;
using BankDataDownloader.Core.Extension;
using BankDataDownloader.Core.Parser;
using BankDataDownloader.Core.Service;
using BankDataDownloader.Core.Service.Impl;
using BankDataDownloader.Data.Entity;
using BankDataDownloader.Data.Entity.BankTransactions;
using BankDataDownloader.Data.Repository;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace BankDataDownloader.Core.DownloadHandler.Impl
{

    public class RaiffeisenDownloadHandler : BankDownloadHandlerBase
    {
        public IPortfolioRepository PortfolioRepository { get; }

        public RaiffeisenDownloadHandler(IBankAccountRepository bankAccountRepository, DbContext dbContext, IKeePassService keePassService, DownloadHandlerConfiguration configuration, IComponentContext componentContext, IPortfolioRepository portfolioRepository) : base(bankAccountRepository, dbContext, keePassService, configuration, componentContext)
        {
            PortfolioRepository = portfolioRepository;
        }

        protected override void Login()
        {
            using (KeePassService.Open())
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
                Browser.FindElement(new ByIdOrName("loginpinform:PIN"))
                    .SendKeys(KeePassEntry.GetString(Constants.DownloadHandler.RaiffeisenPin));

                //final login
                Browser.FindElement(new ByIdOrName("loginpinform:anmeldenPIN")).Click();
            }
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
                var iban = GetAccountLinks()[i].Text.CleanString();
                var bankAccount = BankAccountRepository.GetByIban(iban);
                if (bankAccount == null)
                {
                    bankAccount = new BankAccountEntity
                    {
                        AccountNumber = iban,
                        Iban = iban,
                        BankName = Constants.DownloadHandler.BankNameRaiffeisen,
                        AccountName = i == 0 ? Constants.DownloadHandler.AccountNameGiro : Constants.DownloadHandler.AccountNameSaving
                    };
                    BankAccountRepository.Insert(bankAccount);
                    DbContext.SaveChanges();
                }
                GetAccountLinks()[i].Click();

                Screenshot ss = ((ITakesScreenshot)Browser).GetScreenshot();
                ss.SaveAsFile(Path.Combine(Configuration.DownloadPath, $"{bankAccount.Iban}.png"), System.Drawing.Imaging.ImageFormat.Png);

                SetMaxDateRange();

                Browser.FindElement(
                new ByChained(By.ClassName("serviceButtonArea"),
                    new ByAll(By.ClassName("formControlButton"), By.ClassName("print")))).Click();

                var resultingFile = DownloadCsv(iban);
                DownloadResults.Add(new DownloadResult
                {
                    Account = bankAccount,
                    FileParser =
                        ComponentContext.ResolveNamed<IFileParser>(Constants.UniqueContainerKeys.FileParserRaiffeisen),
                    FilePath = resultingFile,
                    TargetEntity = typeof(RaiffeisenTransactionEntity)
                });
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
                    var portfolioNumber = GetAccountLinks()[i].Text.CleanString();
                    var portfolio = PortfolioRepository.GetByPortfolioNumberAndBankName(portfolioNumber,
                        Constants.DownloadHandler.BankNameRaiffeisen);
                    if (portfolio == null)
                    {
                        portfolio = new PortfolioEntity
                        {
                            PortfolioNumber = portfolioNumber,
                            BankName = Constants.DownloadHandler.BankNameRaiffeisen,
                            AccountName = Constants.DownloadHandler.AccountNameDepot
                        };
                        PortfolioRepository.Insert(portfolio);
                        //TODO check why nothing is persisted here
                        DbContext.SaveChanges();
                    }


                    GetAccountLinks()[i].Click();

                    Screenshot ss = ((ITakesScreenshot)Browser).GetScreenshot();
                    ss.SaveAsFile(Path.Combine(Configuration.DownloadPath, $"{portfolio.PortfolioNumber}.png"), System.Drawing.Imaging.ImageFormat.Png);

                    Browser.FindElement(new ByChained(By.ClassName("serviceButtonArea"), By.LinkText("Daten exportieren"))).Click();

                    var resultingFile = DownloadCsv(portfolioNumber);
                    //DownloadResults.Add(new DownloadResult
                    //{
                    //    Account = portfolio,
                    //    FileParser =
                    //        ComponentContext.ResolveNamed<IFileParser>(Constants.UniqueContainerKeys.FileParserRaiffeisenDepot),
                    //    FilePath = resultingFile,
                    //    TargetEntity = typeof()
                    //});
                    NavigateDepots();

                }
            }
            catch (NoSuchElementException)
            {
            }
        }

        private string DownloadCsv(string fileName)
        {
            var combo =
                new SelectElement(Browser.FindElement(new ByChained(By.ClassName("mainInput"), By.TagName("select"))));
            combo.SelectByValue("CSV");

            var filesPreDownload = Directory.GetFiles(Configuration.DownloadPath);

            Browser.FindElement(By.LinkText("Datei erstellen")).Click();

            Browser.WaitForDownloadToFinishByDirectory(Configuration.DownloadPath);
            var file = Directory.GetFiles(Configuration.DownloadPath).Single(s => !filesPreDownload.Contains(s));
            return Helper.FileRename(file, fileName);
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
