using System.Collections.Generic;

namespace BankManager.Ui.Model.Response
{
    public class ResponseModel
    {
        public object Data { get; set; }
        public IEnumerable<string> Messages { get; set; }
    }
}
