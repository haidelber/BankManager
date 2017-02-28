using System.Deployment.Application;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Navigation;

namespace BankDataDownloader.Ui.Windows
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            ReadAssemblyInfo();
            InitializeComponent();
        }

        public string ProductVersion { get; set; }
        public string AssemblyVersion { get; set; }
        public string DeploymentVersion { get; set; }
        public string Copyright { get; set; }
        public string Product { get; set; }
        public string Description { get; set; }

        private void ReadAssemblyInfo()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            ProductVersion = versionInfo.ProductVersion;
            AssemblyVersion = assembly.GetName().Version.ToString();
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                DeploymentVersion = ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString(4);
            }
            Copyright = versionInfo.LegalCopyright;
            Product = versionInfo.ProductName;
            Description = versionInfo.Comments;
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
