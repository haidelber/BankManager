using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankManager.Common.Model.Configuration;
using BankManager.Core.Model.Import;
using BankManager.Core.Service;
using Microsoft.AspNetCore.Mvc;

namespace BankManager.Ui.Controllers
{
    public class ImportController : ApiController
    {
        public IImportService ImportService { get; }
        public ApplicationConfiguration ApplicationConfiguration { get; }

        public ImportController(IImportService importService, ApplicationConfiguration applicationConfiguration)
        {
            ImportService = importService;
            ApplicationConfiguration = applicationConfiguration;
        }

        [HttpPost("[action]")]
        public IActionResult Run([FromBody]ImportServiceRunModel runModel)
        {
            ImportService.Import(runModel);
            return Json(true);
        }

        [HttpGet("[action]")]
        public IActionResult FileParserConfiguration()
        {
            return Json(GetFileParserConfiguration());
        }

        private IEnumerable<string> GetFileParserConfiguration()
        {
            return ApplicationConfiguration.FileParserConfigurations.Select(pair => pair.Key);
        }
    }
}
