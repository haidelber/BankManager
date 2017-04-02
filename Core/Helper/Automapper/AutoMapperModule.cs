using System.Collections.Generic;
using System.Reflection;
using Autofac;
using AutoMapper;
using NLog;
using Module = Autofac.Module;

namespace BankManager.Core.Helper.Automapper
{
    public class AutoMapperModule : Module
    {
        public readonly Logger Logger = LogManager.GetCurrentClassLogger();

        protected override void Load(ContainerBuilder builder)
        {
            Logger.Info($"Registering {GetType().Name}..");

            var ui = Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(ui).AssignableTo(typeof(Profile)).As<Profile>();
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
