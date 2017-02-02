using System.Windows;
using Autofac;
using BankDataDownloader.Core.Installer;
using BankDataDownloader.Ui.Windows;
using Prism.Autofac;
using Prism.Modularity;

namespace BankDataDownloader.Ui.Installer
{
    public class BankDataDownloaderBootstrapper : AutofacBootstrapper
    {
        protected override void ConfigureContainerBuilder(ContainerBuilder builder)
        {
            base.ConfigureContainerBuilder(builder);
            builder.RegisterInstance(this).AsImplementedInterfaces().SingleInstance();

            new ServiceInstaller().RegisterComponents(builder);
            new ViewInstaller().RegisterComponents(builder);
        }

        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow = Shell as Window;
            if (Application.Current.MainWindow != null) Application.Current.MainWindow.Show();
        }
    }
}