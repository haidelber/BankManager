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
    public class Number26DownloadHandler : BankDownloadHandlerBase
    {
        public ICreditCardAccountRepository CreditCardAccountRepository { get; }

        public Number26DownloadHandler(IBankAccountRepository bankAccountRepository, IPortfolioRepository portfolioRepository, IPortfolioPositionRepository portfolioPositionRepository, IBankTransactionRepository<TransactionEntity> bankTransactionRepository, IKeePassService keePassService, DownloadHandlerConfiguration configuration, IComponentContext componentContext, ICreditCardAccountRepository creditCardAccountRepository, IImportService importService) : base(bankAccountRepository, portfolioRepository, portfolioPositionRepository, bankTransactionRepository, keePassService, configuration, componentContext, importService)
        {
            CreditCardAccountRepository = creditCardAccountRepository;
        }

        protected override void Login()
        {
            using (KeePassService.Open())
            {
                Browser.WaitForJavaScript();
                Browser.FindElement(By.Name("email"))
                    .SendKeys(KeePassEntry.GetUserName());
                Browser.FindElement(By.Name("password"))
                    .SendKeys(KeePassEntry.GetPassword());

                Browser.FindElement(new ByAll(By.TagName("a"), By.ClassName("login"))).Click();
            }
        }

        protected override void Logout()
        {
            Browser.WaitForJavaScript();
            Browser.FindElement(By.ClassName("UIHeader__logout-button")).Click();

            Browser.WaitForJavaScript();
            Browser.FindElement(By.CssSelector(".btn.ok")).Click();
        }

        protected override void NavigateHome()
        {
            Browser.WaitForJavaScript();
            Browser.FindElement(By.ClassName("UIHeader__logo")).Click();
            Browser.WaitForJavaScript();
            var originalHandle = Browser.CurrentWindowHandle;
            Browser.SwitchTo().Alert().Accept();
            Browser.SwitchTo().Window(originalHandle);
            Browser.WaitForJavaScript(5000);
        }

        protected override IEnumerable<FileParserInput> DownloadTransactions()
        {
            var valueParserDe =
                    ComponentContext.ResolveKeyed<IValueParser>(Constants.UniqueContainerKeys.ValueParserGermanDecimal);
            var valueParserEn =
                    ComponentContext.ResolveKeyed<IValueParser>(Constants.UniqueContainerKeys.ValueParserEnglishDecimal);

            Browser.WaitForJavaScript(5000);

            //settings
            Browser.FindElement(By.XPath("//*[@class='UIMenu']/ul/li[4]/a")).Click();
            Browser.WaitForJavaScript(5000);
            var iban = Browser.FindElements(By.ClassName("iban-split")).Select(element => element.Text).Aggregate("", (s, s1) => s + s1).CleanString();
            var balanceString = Browser.FindElement(By.ClassName("UIHeader__account-balance")).Text.ExtractDecimalNumberString();
            decimal balance;
            if (balanceString.Contains("."))
            {
                balance = (decimal)valueParserEn.Parse(balanceString);
            }
            else
            {
                balance = (decimal)valueParserDe.Parse(balanceString);
            }

            var bankAccount = CreditCardAccountRepository.GetByAccountNumberAndBankName(iban, Constants.DownloadHandler.BankNameNumber26);
            if (bankAccount == null)
            {
                bankAccount =
                    CreditCardAccountRepository.Query()
                        .First(
                            entity =>
                                entity.BankName == Constants.DownloadHandler.BankNameNumber26 &&
                                entity.AccountName == Constants.DownloadHandler.AccountNameMasterCard);
                if (bankAccount == null)
                {
                    bankAccount = new CreditCardEntity
                    {
                        AccountNumber = iban,
                        BankName = Constants.DownloadHandler.BankNameNumber26,
                        AccountName = Constants.DownloadHandler.AccountNameMasterCard
                    };
                    CreditCardAccountRepository.Insert(bankAccount);
                }
            }

            NavigateHome();

            TakeScreenshot(iban);

            //Click download button
            Browser.FindElement(By.ClassName("csv")).Click();
            Browser.WaitForJavaScript();
            //Click previous a few times
            Browser.FindElement(By.ClassName("ui-datepicker-today")).Click();
            Browser.WaitForJavaScript(100);
            for (int i = 0; i < 12; i++)
            {
                Browser.FindElement(By.ClassName("ui-datepicker-prev")).Click();
                Browser.WaitForJavaScript(100);
            }
            //Click first day of month
            Browser.FindElement(new ByChained(By.ClassName("ui-datepicker-calendar"), By.XPath("//*[@data-handler='selectDay']"))).Click();

            var resultingFile = DownloadFromWebElement(Browser.FindElement(By.ClassName("ok")), iban);
            yield return new FileParserInput
            {
                OwningEntity = bankAccount,
                FileParser = ComponentContext.ResolveKeyed<IFileParser>(Constants.UniqueContainerKeys.FileParserNumber26),
                FilePath = resultingFile,
                TargetEntity = typeof(Number26TransactionEntity),
                Balance = balance,
                BalanceSelectorFunc =
                     () => BankTransactionRepository.TransactionSumForAccountId(bankAccount.Id)
            };
        }

        protected override void DownloadStatementsAndFiles()
        {
            Browser.FindElement(new ByAll(By.TagName("button"), By.ClassName("balancestatements"))).Click();
            Browser.WaitForJavaScript();

            for (int i = 1; i < GetBalanceStatementLinks().Count; i++)
            {
                var balanceStatementLink = GetBalanceStatementLinks()[i];
                DownloadFromWebElement(balanceStatementLink, balanceStatementLink.GetAttribute("id"));
                Browser.WaitForJavaScript(100);
            }

            Browser.WaitForJavaScript(10000);
        }

        private List<IWebElement> GetBalanceStatementLinks()
        {
            return Browser.FindElements(new ByChained(By.CssSelector(".node.balancestatement"), By.TagName("a"))).ToList();
        }
    }
}