using System.Reflection;
using Autofac;
using Module = Autofac.Module;

namespace BankDataDownloader.Ui.Configuration
{
    public class DefaultViewModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var ui = Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(ui)
                   .Where(t => t.Name.EndsWith("Window"))
                   .AsSelf();
            //cb.RegisterType(typeof(AboutWindow));
            //cb.RegisterType(typeof(MainWindow));
            //cb.RegisterType(typeof(SettingsWindow));
        }
    }
}
