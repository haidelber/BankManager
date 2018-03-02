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
    public class RciDownloadHandler : BankDownloadHandlerBase
    {
        public RciDownloadHandler(IBankAccountRepository bankAccountRepository, IPortfolioRepository portfolioRepository, IPortfolioPositionRepository portfolioPositionRepository, IBankTransactionRepository<TransactionEntity> bankTransactionRepository, IKeePassService keePassService, DownloadHandlerConfiguration configuration, IComponentContext componentContext, IImportService importService) : base(bankAccountRepository, portfolioRepository, portfolioPositionRepository, bankTransactionRepository, keePassService, configuration, componentContext, importService)
        {
        }

        protected override void Login()
        {
            using (KeePassService.Open())
            {
                Browser.FindElement(By.Id("logon-username")).SendKeys(KeePassEntry.GetUserName());
                Browser.WaitForJavaScript();
                Browser.FindElement(By.Id("logon-password")).SendKeys(KeePassEntry.GetPassword());
                Browser.WaitForJavaScript();
                Browser.FindElement(By.XPath("//button[@type='submit']")).Click();
            }
        }

        protected override void Logout()
        {
            throw new NotImplementedException();
        }

        protected override void NavigateHome()
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<FileParserInput> DownloadTransactions()
        {
            Browser.WaitForJavaScript(5000);

            var ibanRaw = Browser.FindElement(By.ClassName("accountText")).Text; //First is Tagesgeld
            var iban = ibanRaw.Split(':')[1].CleanString();
            var balanceStr = Browser.FindElement(new ByChained(By.ClassName("currency"),By.ClassName("amountblock-wrapper"))).Text;
            var valueParser =
                ComponentContext.ResolveKeyed<IValueParser>(Constants.UniqueContainerKeys.ValueParserGermanDecimal);
            var balance = (decimal)valueParser.Parse(balanceStr);
            Browser.FindElement(By.ClassName("accountInfo-wrapper")).Click();
            Browser.WaitForJavaScript();
            
            var bankAccount = BankAccountRepository.GetByIban(iban);
            if (bankAccount == null)
            {
                bankAccount = new BankAccountEntity
                {
                    AccountNumber = iban,
                    Iban = iban,
                    BankName = Constants.DownloadHandler.BankNameRci,
                    AccountName = Constants.DownloadHandler.AccountNameSaving
                };
                BankAccountRepository.Insert(bankAccount);
            }

            TakeScreenshot(iban);

            throw new NotImplementedException();

            //yield return new FileParserInput
            //{
            //    Balance = balance,
            //    BalanceSelectorFunc = () => BankTransactionRepository.TransactionSumForAccountId(bankAccount.Id),
            //    FileParser = ComponentContext.ResolveKeyed<IFileParser>(Constants.UniqueContainerKeys.FileParserRci),
            //    //FilePath = file,
            //    OwningEntity = bankAccount,
            //    TargetEntity = typeof(RciTransactionEntity)
            //};
        }

        protected override void DownloadStatementsAndFiles()
        {
            throw new NotImplementedException();
        }
    }
}