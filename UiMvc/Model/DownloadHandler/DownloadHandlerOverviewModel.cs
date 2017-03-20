using System.Collections.Generic;

namespace BankDataDownloader.Ui.Model.DownloadHandler
{
    public class DownloadHandlerOverviewModel
    {
        public string Key { get; set; }
        public string Url { get; set; }
        public string Path { get; set; }
        public bool Selected { get; set; }
        public string DisplayName { get; set; }
    }

    public class DownloadHandlerRunModel
    {
        public IEnumerable<DownloadHandlerOverviewModel> DownloadHandlerKeys { get; set; }
        public string KeePassPassword { get; set; }
    }
}
