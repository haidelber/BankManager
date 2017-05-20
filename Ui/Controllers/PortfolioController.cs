using System.Collections.Generic;
using BankManager.Core.Model.Porfolio;
using BankManager.Core.Model.Transaction;
using BankManager.Core.Service;
using Microsoft.AspNetCore.Mvc;

namespace BankManager.Ui.Controllers
{
    public class PortfolioController : ApiController
    {
        public IPortfolioService PortfolioService { get; }

        public PortfolioController(IPortfolioService portfolioService)
        {
            PortfolioService = portfolioService;
        }

        [HttpGet("Group")]
        [ProducesResponseType(typeof(IEnumerable<PortfolioGroupModel>), 200)]
        public IActionResult PortfolioGroups()
        {
            return Json(PortfolioService.PortfolioGroups());
        }

        [HttpGet("Group/{portfolioGroupId}/Position")]
        [ProducesResponseType(typeof(IEnumerable<PortfolioPositionModel>), 200)]
        public IActionResult PortfolioGroupPositions([FromRoute]long portfolioGroupId)
        {
            return Json(PortfolioService.PortfolioGroupPositions(portfolioGroupId));
        }

        [HttpGet("Group/{portfolioGroupId}")]
        [ProducesResponseType(typeof(PortfolioGroupModel), 200)]
        public IActionResult GetPortfolioGroup([FromRoute]long portfolioGroupId)
        {
            return Json(PortfolioService.GetPortfolioGroup(portfolioGroupId));
        }

        [HttpPost("Group")]
        [ProducesResponseType(typeof(PortfolioGroupModel), 200)]
        public IActionResult CreateEditPortfolioGroup([FromBody]PortfolioGroupModel model)
        {
            return Json(PortfolioService.CreateEditPortfolioGroup(model));
        }
    }
}
