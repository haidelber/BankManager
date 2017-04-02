using System.Collections.Generic;

namespace BankManager.Core.Model.DownloadHandler
{
    public class DownloadHandlerRunModel
    {
        public IEnumerable<string> DownloadHandlerKeys { get; set; }
        public string KeePassPassword { get; set; }
    }
}