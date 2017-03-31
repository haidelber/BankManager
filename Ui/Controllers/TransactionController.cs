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

        [HttpGet("BankAccount/{id}")]
        public IActionResult BankTransactions([FromRoute]long id)
        {
            return Json(TransactionService.BankTransactions(id));
        }

        [HttpGet("CreditCard/{id}")]
        public IActionResult GetCreditCardTransaction([FromRoute]long id)
        {
            return Json(TransactionService.CreditCardTransactions(id));
        }

        [HttpGet("Portfolio/{id}")]
        public IActionResult GetCurrentPortfolioPosition([FromRoute]long id)
        {
            return Json(TransactionService.PortfolioPositions(id));
        }

        [HttpPost("Portfolio/Sell")]
        public IActionResult CreatePortfolioSalePosition([FromBody] PortfolioPositionModel model)
        {
            return Json(TransactionService.CreatePortfolioSalePosition(model));
        }
    }
}