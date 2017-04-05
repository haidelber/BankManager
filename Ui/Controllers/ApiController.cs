using System.Collections.Generic;
using BankManager.Ui.Model.Response;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace BankManager.Ui.Controllers
{
    [Route("api/[controller]")]
    public abstract class ApiController : Controller
    {
        public readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public override JsonResult Json(object obj)
        {
            //return new JsonResult(new ResponseModel { Data = obj });
            return new JsonResult(obj);
        }
    }
}