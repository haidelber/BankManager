using System.Reflection;
using Autofac;
using BankDataDownloader.Common.Installer;
using BankDataDownloader.Ui.Windows;

namespace BankDataDownloader.Ui.Installer
{
    public class ViewInstaller : IInstaller
    {
        public void RegisterComponents(ContainerBuilder cb)
        {
            var ui = Assembly.GetExecutingAssembly();

            cb.RegisterAssemblyTypes(ui)
                   .Where(t => t.Name.EndsWith("Window"))
                   .AsSelf();
            //cb.RegisterType(typeof(AboutWindow));
            //cb.RegisterType(typeof(MainWindow));
            //cb.RegisterType(typeof(SettingsWindow));
        }
    }
}
