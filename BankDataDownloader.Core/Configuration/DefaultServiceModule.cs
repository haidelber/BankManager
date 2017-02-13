﻿using System.Reflection;
using Autofac;
using BankDataDownloader.Core.DownloadHandler;
using BankDataDownloader.Core.Parser;
using BankDataDownloader.Core.Service.Impl;
using Module = Autofac.Module;

namespace BankDataDownloader.Core.Configuration
{
    public class DefaultServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            var core = Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(core).Where(t => t.Name.EndsWith("Service")).Except<ConfigurationModule>();

            //Configuration Service + configuration model registration
            builder.RegisterModule<ConfigurationModule>();

            builder.RegisterModule<BankDownloadHandlerModule>();
            builder.RegisterModule<ParserModule>();
        }
    }
}
