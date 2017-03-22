using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankManager.Ui.Model.Response
{
    public class ResponseModel
    {
        public object Data { get; set; }
        public IEnumerable<string> Messages { get; set; }
    }
}
