using System.Collections.Generic;

namespace UiAngular.Model.DownloadHandler
{
    public class DownloadHandlerOverviewModel
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Path { get; set; }
        public bool DefaultSelected { get; set; }
    }

    public class DownloadHandlerRunModel
    {
        public IEnumerable<string> DownloadHandlerKeys { get; set; }
        public string KeePassPassword { get; set; }
    }
}
