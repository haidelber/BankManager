using System;
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
    public class DkbDownloadHandler : BankDownloadHandlerBase
    {
        public ICreditCardAccountRepository CreditCardAccountRepository { get; }

        public DkbDownloadHandler(IBankAccountRepository bankAccountRepository, IPortfolioRepository portfolioRepository, IPortfolioPositionRepository portfolioPositionRepository, IBankTransactionRepository<TransactionEntity> bankTransactionRepository, IKeePassService keePassService, DownloadHandlerConfiguration configuration, IComponentContext componentContext, ICreditCardAccountRepository creditCardAccountRepository, IImportService importService) : base(bankAccountRepository, portfolioRepository, portfolioPositionRepository, bankTransactionRepository, keePassService, configuration, componentContext, importService)
        {
            CreditCardAccountRepository = creditCardAccountRepository;
        }

        protected override void Login()
        {
            using (KeePassService.Open())
            {
                Browser.FindElement(By.Id("loginInputSelector")).SendKeys(KeePassEntry.GetUserName());
                Browser.FindElement(By.Id("pinInputSelector")).SendKeys(KeePassEntry.GetPassword());

                Browser.FindElement(By.Id("login")).Submit();
            }
        }

        protected override void Logout()
        {
            Browser.FindElement(By.Id("logout")).Click();
        }

        protected override void NavigateHome()
        {
            Browser.FindElement(By.ClassName("dkb_logo_container")).Click();
        }

        protected override IEnumerable<FileParserInput> DownloadTransactions()
        {
            var downloadResults = new List<FileParserInput>();
            var valueParser =
                    ComponentContext.ResolveKeyed<IValueParser>(Constants.UniqueContainerKeys.ValueParserGermanDecimal);

            //bankaccount
            var iban = GetAccounts()[0].FindElement(By.ClassName("iban")).Text.CleanString();
            var balance = (decimal)valueParser.Parse(
                    GetAccounts()[0].FindElement(new ByChained(By.ClassName("amount"), By.TagName("span"))).Text);
            GetAccounts()[0].FindElement(By.ClassName("evt-paymentTransaction")).Click();

            var bankAccount = BankAccountRepository.GetByIban(iban);
            if (bankAccount == null)
            {
                bankAccount = new BankAccountEntity
                {
                    AccountNumber = iban,
                    Iban = iban,
                    BankName = Constants.DownloadHandler.BankNameDkb,
                    AccountName = Constants.DownloadHandler.AccountNameGiro
                };
                BankAccountRepository.Insert(bankAccount);
            }
            var resultingFile = DownloadAndScreenshot(iban, "[id*=transactionDate]", "[id*=toTransactionDate]");
            downloadResults.Add(new FileParserInput
            {
                OwningEntity = bankAccount,
                FileParser = ComponentContext.ResolveKeyed<IFileParser>(Constants.UniqueContainerKeys.FileParserDkbGiro),
                FilePath = resultingFile,
                TargetEntity = typeof(DkbTransactionEntity),
                UniqueIdGroupingFunc = entity => ((DkbTransactionEntity)entity).AvailabilityDate.Date,
                OrderingFuncs = new List<Func<object, object>> { o => ((DkbTransactionEntity)o).AvailabilityDate.Date, o => ((DkbTransactionEntity)o).Text, o => ((DkbTransactionEntity)o).Amount },
                Balance = balance,
                BalanceSelectorFunc =
                     () => BankTransactionRepository.TransactionSumForAccountId(bankAccount.Id)
            });

            NavigateHome();

            //credit card
            var creditCardNumberMasked = GetAccounts()[1].FindElement(By.XPath("td[1]/div[2]")).Text.CleanString();
            var creditBalance = (decimal)valueParser.Parse(
                    GetAccounts()[1].FindElement(new ByChained(By.ClassName("amount"), By.TagName("span"))).Text);
            GetAccounts()[1].FindElement(By.ClassName("evt-paymentTransaction")).Click();

            var creditCardAccount = CreditCardAccountRepository.GetByAccountNumberAndBankName(creditCardNumberMasked,
                Constants.DownloadHandler.BankNameDkb);
            if (creditCardAccount == null)
            {
                creditCardAccount = new CreditCardEntity
                {
                    AccountNumber = creditCardNumberMasked,
                    CreditCardNumber = null,
                    BankName = Constants.DownloadHandler.BankNameDkb,
                    AccountName = Constants.DownloadHandler.AccountNameVisa
                };
                CreditCardAccountRepository.Insert(creditCardAccount);
            }
            resultingFile = DownloadAndScreenshot(creditCardNumberMasked, "[id*=postingDate]", "[id*=toPostingDate]");
            downloadResults.Add(new FileParserInput
            {
                OwningEntity = creditCardAccount,
                FileParser =
                     ComponentContext.ResolveKeyed<IFileParser>(Constants.UniqueContainerKeys.FileParserDkbCredit),
                FilePath = resultingFile,
                TargetEntity = typeof(DkbCreditTransactionEntity),
                UniqueIdGroupingFunc = entity => ((DkbCreditTransactionEntity)entity).AvailabilityDate.Date,
                OrderingFuncs = new List<Func<object, object>> { o => ((DkbCreditTransactionEntity)o).AvailabilityDate.Date, o => ((DkbCreditTransactionEntity)o).Text, o => ((DkbCreditTransactionEntity)o).Amount },
                Balance = creditBalance,
                BalanceSelectorFunc = () => BankTransactionRepository.TransactionSumForAccountId(creditCardAccount.Id)
            });
            return downloadResults;
        }

        protected override void DownloadStatementsAndFiles()
        {
            GetPostboxMenuItem().Click();
            for (var i = 0; i < GetSubPostboxMenuItems().Count; i++)
            {
                GetSubPostboxMenuItems()[i].Click();

                foreach (var fileLink in Browser.FindElements(By.ClassName("iconSpeichern0")))
                {
                    fileLink.Click();
                }
            }
        }

        private List<IWebElement> GetAccounts()
        {
            return Browser.FindElements(new ByChained(By.ClassName("financialStatusTable"), By.ClassName("mainRow"))).ToList();
        }

        private string DownloadAndScreenshot(string filename, string cssSelectorFromDate, string cssSelectorToDate)
        {
            TakeScreenshot(filename);

            SetMaxDateRange(cssSelectorFromDate, cssSelectorToDate);
            return
                DownloadFromWebElement(
                    Browser.FindElement(new ByChained(By.ClassName("evt-csvExport"), By.ClassName("iconExport0"))),
                    filename);
        }

        private void SetMaxDateRange(string cssSelectorFromDate, string cssSelectorToDate)
        {
            var dateFormatString = "dd.MM.yyyy";

            var startRange = Browser.FindElement(new ByAll(By.TagName("input"), By.CssSelector(cssSelectorFromDate)));
            startRange.Clear();
            startRange.SendKeys(DateTime.Today.AddYears(-5).ToString(dateFormatString));

            var endRange = Browser.FindElement(new ByAll(By.TagName("input"), By.CssSelector(cssSelectorToDate)));
            endRange.Clear();
            endRange.SendKeys(DateTime.Today.ToString(dateFormatString));

            Browser.FindElement(By.Id("searchbutton")).Click();
            //Date has been adapted now click again
            Browser.FindElement(By.Id("searchbutton")).Click();
        }

        private IWebElement GetPostboxMenuItem()
        {
            return Browser.FindElement(new ByChained(By.Id("menu"), By.LinkText("Postfach")));
        }

        private List<IWebElement> GetSubPostboxMenuItems()
        {
            return GetPostboxMenuItem().FindElements(By.XPath("..//ul//a")).ToList();
        }
    }
}