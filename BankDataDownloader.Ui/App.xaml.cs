using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using BankDataDownloader.Common.Properties;
using BankDataDownloader.Ui.Installer;

namespace BankDataDownloader.Ui
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var bs = new BankDataDownloaderBootstrapper();
            bs.Run();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iso639_1">the 2 letter language code</param>
        public void ChangeUiLanguage(string iso639_1)
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(SettingsHandler.Instance.LanguageIso639_1);

            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement),
             new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentUICulture.IetfLanguageTag)));
        }
    }
}
