using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BankManager.Ui.Controllers
{
    [Route("api/[controller]")]
    public abstract class ApiController : Controller
    {
        public override JsonResult Json(object obj)
        {
            //return new JsonResult(new ResponseModel { Data = obj });
            return new JsonResult(obj);
        }
    }
}