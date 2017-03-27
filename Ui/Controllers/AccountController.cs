﻿using System.Collections.Generic;
using AutoMapper;
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
        public IActionResult BankAccounts()
        {
            return Json(AccountService.BankAccounts());
        }

        [HttpGet("CreditCard")]
        public IActionResult CreditCards()
        {
            return Json(AccountService.CreditCards());
        }

        [HttpGet("Portfolio")]
        public IActionResult Portfolios()
        {
            return Json(AccountService.Portfolios());
        }

        [HttpGet("BankAccount/{id}")]
        public IActionResult BankAccount(long id)
        {
            return Json(AccountService.BankAccount(id));
        }

        [HttpGet("CreditCard/{id}")]
        public IActionResult CreditCard(long id)
        {
            return Json(AccountService.CreditCard(id));
        }

        [HttpGet("Portfolio/{id}")]
        public IActionResult Portfolio(long id)
        {
            return Json(AccountService.Portfolio(id));
        }
    }
}