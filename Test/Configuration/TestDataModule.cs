using System;
using System.Reflection;
using Autofac;
using BankDataDownloader.Common.Model.Configuration;
using BankDataDownloader.Data;
using BankDataDownloader.Data.Configuration;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace BankDataDownloader.Test.Configuration
{
    public class TestDataModule : DataModuleBase
    {
        protected override void RegisterContext(ContainerBuilder builder)
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseSqlite(connection)
                .Options;
            builder.RegisterInstance(options).SingleInstance();
            //Automatically takes constructor with most params
            builder.RegisterType<DataContext>().AsSelf().As<DbContext>().SingleInstance();
        }
    }
}