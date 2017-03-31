using System.Collections.Generic;
using AutoMapper;
using BankDataDownloader.Core.Model.Account;
using BankDataDownloader.Core.Service;
using BankDataDownloader.Data.Repository;
using Microsoft.AspNetCore.Mvc;

namespace BankManager.Ui.Controllers
{
    public class AccountController : ApiController
    {
        public IAccountService AccountService { get; }

        public AccountController(IAccountService accountService)
        {
            AccountService = accountService;
        }

        [HttpGet("BankAccount")]
        [ProducesResponseType(typeof(IEnumerable<BankAccountModel>), 200)]
        public IActionResult BankAccounts()
        {
            return Json(AccountService.BankAccounts());
        }

        [HttpGet("CreditCard")]
        [ProducesResponseType(typeof(IEnumerable<CreditCardAccountModel>), 200)]
        public IActionResult CreditCards()
        {
            return Json(AccountService.CreditCards());
        }

        [HttpGet("Portfolio")]
        [ProducesResponseType(typeof(IEnumerable<PortfolioModel>), 200)]
        public IActionResult Portfolios()
        {
            return Json(AccountService.Portfolios());
        }

        [HttpGet("BankAccount/{id}")]
        [ProducesResponseType(typeof(BankAccountModel), 200)]
        public IActionResult BankAccount([FromRoute]long id)
        {
            return Json(AccountService.BankAccount(id));
        }

        [HttpGet("CreditCard/{id}")]
        [ProducesResponseType(typeof(CreditCardAccountModel), 200)]
        public IActionResult CreditCard([FromRoute]long id)
        {
            return Json(AccountService.CreditCard(id));
        }

        [HttpGet("Portfolio/{id}")]
        [ProducesResponseType(typeof(PortfolioModel), 200)]
        public IActionResult Portfolio([FromRoute]long id)
        {
            return Json(AccountService.Portfolio(id));
        }

        [HttpPost("BankAccount")]
        [ProducesResponseType(typeof(BankAccountModel), 200)]
        public IActionResult CreateEditBankAccount([FromBody]BankAccountModel model)
        {
            return Json(AccountService.CreateEditBankAccount(model));
        }

        [HttpPost("CreditCard")]
        [ProducesResponseType(typeof(CreditCardAccountModel), 200)]
        public IActionResult CreateEditCreditCard([FromBody]CreditCardAccountModel model)
        {
            return Json(AccountService.CreateEditCreditCard(model));
        }

        /// <summary>
        /// Creates a new portfolio if the given model lacks an Id otherwise the portfolio with the given Id is updated.
        /// </summary>
        /// <param name="model">the portfolio</param>
        /// <returns>the edited/created portfolio</returns>
        /// <response code="200">Returns the newly created/edited item</response>
        [HttpPost("Portfolio")]
        [ProducesResponseType(typeof(PortfolioModel), 200)]
        public IActionResult CreateEditPortfolio([FromBody]PortfolioModel model)
        {
            return Json(AccountService.CreateEditPortfolio(model));
        }
    }
}