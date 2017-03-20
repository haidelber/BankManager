using System.Collections.Generic;
using System.Reflection;
using Autofac;
using AutoMapper;
using Module = Autofac.Module;

namespace BankDataDownloader.Ui.Helper.Automapper
{
    public class AutoMapperModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var ui = Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(ui).AssignableTo(typeof(Profile));
            builder.Register(c => new MapperConfiguration(cfg =>
            {
                foreach (var profile in c.Resolve<IEnumerable<Profile>>())
                {
                    cfg.AddProfile(profile);
                }
                cfg.CreateMissingTypeMaps = true;
            })).AsSelf().SingleInstance();

            builder.Register(c => c.Resolve<MapperConfiguration>().CreateMapper()).As<IMapper>().InstancePerLifetimeScope();
        }
    }
}
