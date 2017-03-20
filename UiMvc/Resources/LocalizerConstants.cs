using System.Collections.Generic;
using System.Globalization;

namespace BankDataDownloader.Ui.Resources
{
    public static class LocalizerConstants
    {
        public static readonly List<CultureInfo> SupportedCultures = new List<CultureInfo> { new CultureInfo("en"), new CultureInfo("de") };
        public static List<CultureInfo> SupportedUiCultures => SupportedCultures;

        public static class Common
        {

        }
        public static class Download
        {
            public static class Index
            {
                public const string Title = "Download.Index.Title";
            }
        }
    }
}
