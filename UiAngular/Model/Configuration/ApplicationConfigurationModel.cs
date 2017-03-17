using System.Collections.Generic;

namespace BankDataDownloader.Ui.Model.Configuration
{
    public class ApplicationConfigurationModel
    {
        public IDictionary<string, DownloadHandlerConfigurationModel> DownloadHandlerConfigurations { get; set; }
        public IDictionary<string, FileParserConfigurationModel> FileParserConfigurations { get; set; }
        public KeePassConfigurationModel KeePassConfiguration { get; set; }
        public DatabaseConfigurationModel DatabaseConfiguration { get; set; }
        public UiConfigurationModel UiConfiguration { get; set; }
    }
}