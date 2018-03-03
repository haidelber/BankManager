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
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace BankManager.Core.DownloadHandler.Impl
{

    public class RaiffeisenDownloadHandler : BankDownloadHandlerBase
    {
        public RaiffeisenDownloadHandler(ILogger<RaiffeisenDownloadHandler> logger, IBankAccountRepository bankAccountRepository, IPortfolioRepository portfolioRepository, IPortfolioPositionRepository portfolioPositionRepository, IBankTransactionRepository<TransactionEntity> bankTransactionRepository, IKeePassService keePassService, DownloadHandlerConfiguration configuration, IComponentContext componentContext, IImportService importService) : base(logger, bankAccountRepository, portfolioRepository, portfolioPositionRepository, bankTransactionRepository, keePassService, configuration, componentContext, importService)
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

                Browser.FindElement(By.LinkText("Jetzt Mein ELBA starten")).Click();
            }
        }

        protected override void Logout()
        {
            Browser.FindElement(By.ClassName("logout")).Click();
        }

        protected override void NavigateHome()
        {
            Browser.FindElement(By.XPath("//a[@data-test='button-home']")).Click();
        }

        protected override IEnumerable<FileParserInput> DownloadTransactions()
        {
            var downloadResults = new List<FileParserInput>();

            Browser.WaitForJavaScript(12000);
            Browser.FindElement(By.XPath("//*[@data-test='main-nav-kontozentrale']")).Click();
            Browser.WaitForJavaScript(2000);

            //fist ist cumulative account
            for (var i = 1; i < GetAccountLinks().Count; i++)
            {
                GetAccountLinks()[i].Click();
                var valueParser =
                    ComponentContext.ResolveKeyed<IValueParser>(Constants.UniqueContainerKeys.ValueParserGermanDecimal);
                var balance = GetAccountBalance().Text.CleanNumberStringFromOther();
                var infoBox = Browser.FindElement(By.ClassName("info-box"));
                var iban = infoBox.FindElement(By.TagName("h2")).Text.CleanString();
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

                TakeScreenshot(iban);

                var resultingFile = DownloadFromWebElement(Browser.FindElement(By.ClassName("icon-csv")), iban);
                downloadResults.Add(new FileParserInput
                {
                    OwningEntity = bankAccount,
                    FileParser =
                        ComponentContext.ResolveKeyed<IFileParser>(Constants.UniqueContainerKeys.FileParserRaiffeisenGiro),
                    FilePath = resultingFile,
                    TargetEntity = typeof(RaiffeisenTransactionEntity),
                    Balance = (decimal)valueParser.Parse(balance),
                    BalanceSelectorFunc =
                        () => BankTransactionRepository.TransactionSumForAccountId(bankAccount.Id)
                });
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
            Browser.FindElement(By.XPath("//*[@data-test='main-nav-depotzentrale']")).Click();
        }

        private IEnumerable<FileParserInput> DownloadDepots()
        {
            var downloadResults = new List<FileParserInput>();
            try
            {
                NavigateDepots();
                Browser.WaitForJavaScript();

                for (var i = 1; i < GetAccountLinks().Count; i++)
                {
                    GetAccountLinks()[i].Click();
                    Browser.WaitForJavaScript(2000);
                    var portfolioNumber = Browser.FindElement(new ByChained(By.ClassName("info-box"), By.TagName("h3"))).Text.CleanString();
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

                    TakeScreenshot(portfolio.PortfolioNumber);

                    var resultingFile = DownloadFromWebElement(Browser.FindElement(By.ClassName("icon-csv")), portfolioNumber);
                    downloadResults.Add(new FileParserInput
                    {
                        OwningEntity = portfolio,
                        FileParser =
                            ComponentContext.ResolveKeyed<IFileParser>(Constants.UniqueContainerKeys.FileParserRaiffeisenDepot),
                        FilePath = resultingFile,
                        TargetEntity = typeof(RaiffeisenPositionEntity)
                    });
                }
            }
            catch (NoSuchElementException)
            {
            }
            return downloadResults;
        }

        private List<IWebElement> GetAccountLinks()
        {
            return Browser.FindElements(By.XPath("//li[contains(@data-test,'page-tab-')]")).ToList();
        }

        private IWebElement GetAccountBalance()
        {
            return Browser.FindElement(By.XPath("//zv-betrag[@betrag='vm.konto.kontostand']"));
            //.FindElement(new ByChained(By.Name("span"), By.Name("span")));
        }
    }
}
