using AutoMapper;
using BankDataDownloader.Core.Model.Transaction;
using BankDataDownloader.Core.Service;
using Microsoft.AspNetCore.Mvc;

namespace BankManager.Ui.Controllers
{
    public class TransactionController : ApiController
    {
        public ITransactionService TransactionService { get; }
        public IMapper Mapper { get; }

        public TransactionController(IMapper mapper, ITransactionService transactionService)
        {
            Mapper = mapper;
            TransactionService = transactionService;
        }

        [HttpGet("Cumulative/Account")]
        public IActionResult CumulativeAccountTransactions()
        {
            return Json(TransactionService.CumulativeAccountTransactions());
        }

        [HttpGet("Aggregated/Account")]
        public IActionResult MonthlyAggregatedAccountCapital()
        {
            return Json(TransactionService.MonthlyAggregatedAccountCapital());
        }

        [HttpGet("Cumulative/Portfolio")]
        public IActionResult CumulativePortfolioPosition()
        {
            return Json(TransactionService.CumulativePortfolioPosition());
        }

        [HttpGet("Aggregated/Portfolio")]
        public IActionResult MontlyAggregatedPortfolioCapital()
        {
            return Json(TransactionService.MontlyAggregatedPortfolioCapital());
        }

        [HttpGet("BankAccount")]
        public IActionResult GetBankTransactionsForAccountId([FromQuery]long accountId)
        {
            return Json(TransactionService.GetBankTransactionsForAccountId(accountId));
        }

        [HttpGet("CreditCard")]
        public IActionResult GetCreditCardTransactionsForAccountId([FromQuery]long accountId)
        {
            return Json(TransactionService.GetCreditCardTransactionsForAccountId(accountId));
        }

        [HttpGet("Portfolio")]
        public IActionResult GetPortfolioPositionsForPortfolioId([FromQuery]long portfolioId)
        {
            return Json(TransactionService.GetPortfolioPositionsForPortfolioId(portfolioId));
        }

        [HttpDelete("BankAccount/{id}")]
        public IActionResult DeleteBankTransaction([FromRoute]long id)
        {
            return Json(TransactionService.DeleteBankTransaction(id));
        }

        [HttpDelete("CreditCard/{id}")]
        public IActionResult DeleteCreditCardTransaction([FromRoute]long id)
        {
            return Json(TransactionService.DeleteCreditCardTransaction(id));
        }

        [HttpDelete("Portfolio/{id}")]
        public IActionResult DeletePortfolioPosition([FromRoute]long id)
        {
            return Json(TransactionService.DeletePortfolioPosition(id));
        }

        [HttpPost("Portfolio/Sell")]
        public IActionResult CreatePortfolioSalePosition([FromBody] PortfolioPositionModel model)
        {
            return Json(TransactionService.CreatePortfolioSalePosition(model));
        }
    }
}