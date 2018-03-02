using System.Collections.Generic;
using System.Globalization;
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
using OpenQA.Selenium.Support.UI;

namespace BankManager.Core.DownloadHandler.Impl
{

    public class RaiffeisenDownloadHandler : BankDownloadHandlerBase
    {
        public RaiffeisenDownloadHandler(IBankAccountRepository bankAccountRepository, IPortfolioRepository portfolioRepository, IPortfolioPositionRepository portfolioPositionRepository, IBankTransactionRepository<TransactionEntity> bankTransactionRepository, IKeePassService keePassService, DownloadHandlerConfiguration configuration, IComponentContext componentContext, IImportService importService) : base(bankAccountRepository, portfolioRepository, portfolioPositionRepository, bankTransactionRepository, keePassService, configuration, componentContext, importService)
        {
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
                    .SendKeys(KeePassEntry.GetString(Configuration.AdditionalKeePassFields[Constants.DownloadHandler.RaiffeisenPin]));

                //final login
                Browser.FindElement(new ByIdOrName("loginpinform:anmeldenPIN")).Click();

                Browser.FindElement(By.LinkText("Gewohntes Raiffeisen ELBA-internet starten")).Click();
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

        protected override IEnumerable<FileParserInput> DownloadTransactions()
        {
            var downloadResults = new List<FileParserInput>();
            for (var i = 0; i < GetAccountLinks().Count; i++)
            {
                var iban = GetAccountLinks()[i].Text.CleanString();
                var valueParser =
                    ComponentContext.ResolveKeyed<IValueParser>(Constants.UniqueContainerKeys.ValueParserGermanDecimal);
                var balance = GetAccountBalance()[i].Text.CleanNumberStringFromOther();
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
                }
                GetAccountLinks()[i].Click();

                TakeScreenshot(iban);

                SetMaxDateRange();

                Browser.FindElement(
                new ByChained(By.ClassName("serviceButtonArea"),
                    new ByAll(By.ClassName("formControlButton"), By.ClassName("print")))).Click();

                var resultingFile = DownloadCsv(iban);
                downloadResults.Add(new FileParserInput
                {
                    OwningEntity = bankAccount,
                    FileParser =
                        ComponentContext.ResolveKeyed<IFileParser>(Constants.UniqueContainerKeys.FileParserRaiffeisenGiro),
                    FilePath = resultingFile,
                    TargetEntity = typeof(RaiffeisenTransactionEntity),
                    Balance = (decimal) valueParser.Parse(balance),
                    BalanceSelectorFunc =
                        () => BankTransactionRepository.TransactionSumForAccountId(bankAccount.Id)
                });
                NavigateHome();
            }
            downloadResults.AddRange(DownloadDepots());
            return downloadResults;
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

        private IEnumerable<FileParserInput> DownloadDepots()
        {
            var downloadResults = new List<FileParserInput>();
            try
            {
                NavigateDepots();

                for (var i = 0; i < GetAccountLinks().Count; i++)
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
                    }

                    GetAccountLinks()[i].Click();

                    TakeScreenshot(portfolio.PortfolioNumber);

                    Browser.FindElement(new ByChained(By.ClassName("serviceButtonArea"), By.LinkText("Daten exportieren"))).Click();

                    var resultingFile = DownloadCsv(portfolioNumber);
                    downloadResults.Add(new FileParserInput
                    {
                        OwningEntity = portfolio,
                        FileParser =
                            ComponentContext.ResolveKeyed<IFileParser>(Constants.UniqueContainerKeys.FileParserRaiffeisenDepot),
                        FilePath = resultingFile,
                        TargetEntity = typeof(RaiffeisenPositionEntity)
                    });
                    NavigateDepots();
                }
            }
            catch (NoSuchElementException)
            {
            }
            return downloadResults;
        }

        private string DownloadCsv(string fileName)
        {
            var combo =
                new SelectElement(Browser.FindElement(new ByChained(By.ClassName("mainInput"), By.TagName("select"))));
            combo.SelectByValue("CSV");

            return DownloadFromWebElement(Browser.FindElement(By.LinkText("Datei erstellen")), fileName);
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

        private List<IWebElement> GetAccountBalance()
        {
            return Browser.FindElements(
                new ByChained(
                    By.ClassName("kontoTable"),
                    By.TagName("tbody"),
                    By.TagName("tr"),
                    By.XPath("td[4]"),
                    By.TagName("span")
                    )).ToList();
        }
    }
}
