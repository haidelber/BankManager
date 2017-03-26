namespace BankDataDownloader.Core.Model.DownloadHandler
{
    public class DownloadHandlerOverviewModel
    {
        public string Key { get; set; }
        public string Url { get; set; }
        public string Path { get; set; }
        public bool Selected { get; set; }
        public string DisplayName { get; set; }
    }
}
