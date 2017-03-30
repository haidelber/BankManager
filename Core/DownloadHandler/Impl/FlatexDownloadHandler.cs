using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using BankDataDownloader.Common;
using BankDataDownloader.Common.Extensions;
using BankDataDownloader.Common.Model.Configuration;
using BankDataDownloader.Core.Extension;
using BankDataDownloader.Core.Model;
using BankDataDownloader.Core.Model.FileParser;
using BankDataDownloader.Core.Parser;
using BankDataDownloader.Core.Service;
using BankDataDownloader.Data.Entity;
using BankDataDownloader.Data.Entity.BankTransactions;
using BankDataDownloader.Data.Repository;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace BankDataDownloader.Core.DownloadHandler.Impl
{
    public class FlatexDownloadHandler : BankDownloadHandlerBase
    {
        public FlatexDownloadHandler(IBankAccountRepository bankAccountRepository, IPortfolioRepository portfolioRepository, IPortfolioPositionRepository portfolioPositionRepository, IBankTransactionRepository bankTransactionRepository, IKeePassService keePassService, DownloadHandlerConfiguration configuration, IComponentContext componentContext, IImportService importService) : base(bankAccountRepository, portfolioRepository, portfolioPositionRepository, bankTransactionRepository, keePassService, configuration, componentContext,importService)
        {
        }

        protected override void Login()
        {
            using (KeePassService.Open())
            {
                Browser.FindElement(By.Id("loginForm_userId")).SendKeys(KeePassEntry.GetUserName());
                Browser.FindElement(By.Id("loginForm_pin")).SendKeys(KeePassEntry.GetPassword());
                Browser.WaitForJavaScript();
                //Browser.FindElement(By.Id("loginFormData_loginForm")).Submit();
                Browser.FindElement(By.Id("loginForm_loginButton")).Click();
            }
        }

        protected override void Logout()
        {
            var actions = new Actions(Browser);
            var element = Browser.FindElement(By.ClassName("LogoutArea"));
            actions.MoveToElement(element);
            actions.Perform();
            element.Click();
        }

        protected override void NavigateHome()
        {
            Browser.Navigate().GoToUrl("https://konto.flatex.at/banking-flatex.at/accountOverviewFormAction.do");
        }

        protected override IEnumerable<FileParserInput> DownloadTransactions()
        {
            Browser.WaitForJavaScript();
            //Konto
            var accountNumber = Browser.FindElements(By.ClassName("C0"))[3].Text;
            var iban = GetIbanFromOldBanking();

            //go to account transactions
            Browser.Navigate().GoToUrl("https://konto.flatex.at/banking-flatex.at/accountPostingsFormAction.do");
            var balanceString = Browser.FindElements(new ByChained(By.ClassName("Details"), By.ClassName("Value")))[0].Text.ExtractDecimalNumberString();
            var valueParserDe =
                    ComponentContext.ResolveKeyed<IValueParser>(Constants.UniqueContainerKeys.ValueParserGermanDecimal);
            var balance = (decimal)valueParserDe.Parse(balanceString);
            //Date range -1 year
            var fromDate = Browser.FindElement(By.Id("accountPostingsForm_dateRangeComponent_startDate"));
            fromDate.SetAttribute("value", DateTime.Today.AddYears(-1).ToString("dd.MM.yyyy"));
            Browser.FindElement(By.Id("accountPostingsForm_applyFilterButton")).Click();
            //excel download
            TakeScreenshot(iban);
            var resultingFile = DownloadFromWebElement(Browser.FindElement(By.Id("accountPostingsForm_excelExportButton")), iban);
            //check for account or create new
            var bankAccount = BankAccountRepository.GetByIban(iban);
            if (bankAccount == null)
            {
                bankAccount = new BankAccountEntity
                {
                    AccountNumber = accountNumber,
                    Iban = iban,
                    BankName = Constants.DownloadHandler.BankNameFlatex,
                    AccountName = Constants.DownloadHandler.AccountNameGiro
                };
                BankAccountRepository.Insert(bankAccount);
            }
            yield return new FileParserInput
            {
                OwningEntity = bankAccount,
                FileParser = ComponentContext.ResolveKeyed<IFileParser>(Constants.UniqueContainerKeys.FileParserFlatexGiro),
                FilePath = resultingFile,
                TargetEntity = typeof(FlatexTransactionEntity),
                Balance = balance,
                BalanceSelectorFunc =
                    () => BankTransactionRepository.GetAllForAccountId(bankAccount.Id).Sum(entity => entity.Amount)
            };

            //Depot
            NavigateHome();
            var portfolioNumber = Browser.FindElements(By.ClassName("C0"))[5].Text;
            Browser.Navigate().GoToUrl("https://konto.flatex.at/banking-flatex.at/depositStatementFormAction.do");
            TakeScreenshot(portfolioNumber);
            resultingFile = DownloadFromWebElement(
                Browser.FindElement(By.Id("depositStatementForm_excelExportButton")), accountNumber);

            var portfolio = PortfolioRepository.GetByPortfolioNumberAndBankName(portfolioNumber,
                        Constants.DownloadHandler.BankNameFlatex);
            if (portfolio == null)
            {
                portfolio = new PortfolioEntity
                {
                    PortfolioNumber = portfolioNumber,
                    BankName = Constants.DownloadHandler.BankNameFlatex,
                    AccountName = Constants.DownloadHandler.AccountNameDepot
                };
                PortfolioRepository.Insert(portfolio);
            }

            yield return new FileParserInput
            {
                OwningEntity = portfolio,
                FileParser = ComponentContext.ResolveKeyed<IFileParser>(Constants.UniqueContainerKeys.FileParserFlatexDepot),
                FilePath = resultingFile,
                TargetEntity = typeof(FlatexPortfolioPositionEntity)
            };
        }

        private string GetIbanFromOldBanking()
        {
            //go to old banking for iban
            Browser.Navigate().GoToUrl("https://konto.flatex.at/banking-flatex.at/serviceOverviewFormAction.do");
            Browser.FindElement(By.Id("serviceOverviewForm_accountConnectionsButton")).Click();
            Browser.WaitForJavaScript();
            Browser.FindElement(By.Id("webFilialeClassicSSOForm_okButton")).Click();
            //switch window
            var originalHandle = Browser.CurrentWindowHandle;
            foreach (var windowHandle in Browser.WindowHandles)
            {
                if (!windowHandle.Equals(originalHandle))
                {
                    Browser.SwitchTo().Window(windowHandle);
                    break;
                }
            }
            var iban = Browser.FindElement(By.ClassName("ibanValue")).Text.CleanString();
            Browser.Navigate().GoToUrl("https://konto.flatex.at/onlinebanking-flatex.at/logoutFormAction.do");
            //switch back to original window
            Browser.Close();
            Browser.SwitchTo().Window(originalHandle);

            return iban;
        }

        protected override void DownloadStatementsAndFiles()
        {
            Browser.Navigate().GoToUrl("https://konto.flatex.at/banking-flatex.at/documentArchiveListFormAction.do");
            Browser.FindElement(By.Id("documentArchiveListForm_dateRangeComponent_startDate"))
                .SetAttribute("value", DateTime.Today.AddMonths(-3).ToString("dd.MM.yyyy"));
            Browser.FindElement(By.Id("documentArchiveListForm_applyFilterButton")).Click();
            Browser.WaitForJavaScript();
            for (var i = 0; i < GetFiles().Count; i++)
            {
                GetFiles()[i].Click();
                Browser.WaitForJavaScript();
                var originalHandle = Browser.CurrentWindowHandle;
                foreach (var windowHandle in Browser.WindowHandles)
                {
                    if (!windowHandle.Equals(originalHandle))
                    {
                        Browser.SwitchTo().Window(windowHandle);
                        break;
                    }
                }
                Browser.WaitForJavaScript(5000);
                Browser.FindElement(By.Id("download")).Click();
                Browser.Close();
                Browser.SwitchTo().Window(originalHandle);
            }
        }

        private ReadOnlyCollection<IWebElement> GetFiles()
        {
            return Browser.FindElements(new ByChained(By.ClassName("C2"), By.ClassName("Ellipsis")));
        }
    }
}
