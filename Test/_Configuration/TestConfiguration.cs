using System;
using System.Configuration;
using System.Globalization;
using System.Security;
using BankManager.Common.Extensions;

namespace BankManager.Test._Configuration
{
    public static class TestConfiguration
    {
        public static decimal ParseDecimal(string str)
        {
            return decimal.Parse(str, CultureInfo.InvariantCulture);
        }

        public static class DownloadHandler
        {
            public static class Dkb
            {
                public static readonly string Path = ConfigurationManager.AppSettings["DownloadHandler.Dkb.Path"];

                public static readonly string CreditAccountNumber = ConfigurationManager.AppSettings["DownloadHandler.Dkb.CreditAccountNumber"];
                public static readonly decimal CreditBalance = ParseDecimal(ConfigurationManager.AppSettings["DownloadHandler.Dkb.CreditBalance"]);
                public static readonly int CreditCount = int.Parse(ConfigurationManager.AppSettings["DownloadHandler.Dkb.CreditCount"]);

                public static readonly string GiroIban = ConfigurationManager.AppSettings["DownloadHandler.Dkb.GiroIban"];
                public static readonly decimal GiroBalance = ParseDecimal(ConfigurationManager.AppSettings["DownloadHandler.Dkb.GiroBalance"]);
                public static readonly int GiroCount = int.Parse(ConfigurationManager.AppSettings["DownloadHandler.Dkb.GiroCount"]);
            }

            public static class Flatex
            {
                public static readonly string Path = ConfigurationManager.AppSettings["DownloadHandler.Flatex.Path"];

                public static readonly string PortfolioNumber = ConfigurationManager.AppSettings["DownloadHandler.Flatex.PortfolioNumber"];
                public static readonly int PortfolioCount = int.Parse(ConfigurationManager.AppSettings["DownloadHandler.Flatex.PortfolioCount"]);

                public static readonly string GiroIban = ConfigurationManager.AppSettings["DownloadHandler.Flatex.GiroIban"];
                public static readonly decimal GiroBalance = ParseDecimal(ConfigurationManager.AppSettings["DownloadHandler.Flatex.GiroBalance"]);
                public static readonly int GiroCount = int.Parse(ConfigurationManager.AppSettings["DownloadHandler.Flatex.GiroCount"]);
            }

            public static class Number26
            {
                public static readonly string Path = ConfigurationManager.AppSettings["DownloadHandler.Number26.Path"];

                public static readonly string Iban = ConfigurationManager.AppSettings["DownloadHandler.Number26.Iban"];
                public static readonly decimal Balance = ParseDecimal(ConfigurationManager.AppSettings["DownloadHandler.Number26.Balance"]);
                public static readonly int Count = int.Parse(ConfigurationManager.AppSettings["DownloadHandler.Number26.Count"]);
            }

            public static class PayPal
            {
                public static readonly string Path = ConfigurationManager.AppSettings["DownloadHandler.PayPal.Path"];

                public static readonly decimal Balance = ParseDecimal(ConfigurationManager.AppSettings["DownloadHandler.PayPal.Balance"]);
                public static readonly int Count = int.Parse(ConfigurationManager.AppSettings["DownloadHandler.PayPal.Count"]);
            }

            public static class Raiffeisen
            {
                public static readonly string Path = ConfigurationManager.AppSettings["DownloadHandler.Raiffeisen.Path"];

                public static readonly string GiroIban = ConfigurationManager.AppSettings["DownloadHandler.Raiffeisen.GiroIban"];
                public static readonly decimal GiroBalance = ParseDecimal(ConfigurationManager.AppSettings["DownloadHandler.Raiffeisen.GiroBalance"]);
                public static readonly int GiroCount = int.Parse(ConfigurationManager.AppSettings["DownloadHandler.Raiffeisen.GiroCount"]);
            }

            public static class Rci
            {
                public static readonly string Path = ConfigurationManager.AppSettings["DownloadHandler.Rci.Path"];

                public static readonly string Iban = ConfigurationManager.AppSettings["DownloadHandler.Rci.Iban"];
                public static readonly decimal Balance = ParseDecimal(ConfigurationManager.AppSettings["DownloadHandler.Rci.Balance"]);
                public static readonly int Count = int.Parse(ConfigurationManager.AppSettings["DownloadHandler.Rci.Count"]);
            }
        }

        public static class Parser
        {
            public static readonly string RaiffeisenPath = ConfigurationManager.AppSettings["Parser.RaiffeisenPath"];
            public static readonly string Number26Path = ConfigurationManager.AppSettings["Parser.Number26Path"];
            public static readonly string PayPalPath = ConfigurationManager.AppSettings["Parser.PayPalPath"];
            public static readonly string DkbGiroPath = ConfigurationManager.AppSettings["Parser.DkbGiroPath"];
            public static readonly string DkbCreditPath = ConfigurationManager.AppSettings["Parser.DkbCreditPath"];
            public static readonly string RciPath = ConfigurationManager.AppSettings["Parser.RciPath"];
            public static readonly string FlatexGiroPath = ConfigurationManager.AppSettings["Parser.FlatexGiroPath"];
            public static readonly string FlatexDepotPath = ConfigurationManager.AppSettings["Parser.FlatexDepotPath"];
        }

        public static class Database
        {
            public static readonly string Path = ConfigurationManager.AppSettings["Database.Path"];
        }

        public static class KeePass
        {
            public static readonly string Path = ConfigurationManager.AppSettings["KeePass.Path"];
            public static SecureString Password => ConfigurationManager.AppSettings["KeePass.Password"].ConvertToSecureString();

            public static readonly string RaiffeisenUuid = ConfigurationManager.AppSettings["KeePass.RaiffeisenUuid"];
            public static readonly string DkbUuid = ConfigurationManager.AppSettings["KeePass.DkbUuid"];
            public static readonly string Number26Uuid = ConfigurationManager.AppSettings["KeePass.Number26Uuid"];
            public static readonly string RciUuid = ConfigurationManager.AppSettings["KeePass.RciUuid"];
            public static readonly string SantanderUuid = ConfigurationManager.AppSettings["KeePass.SantanderUuid"];
            public static readonly string PayPalUuid = ConfigurationManager.AppSettings["KeePass.PayPalUuid"];
            public static readonly string FlatexUuid = ConfigurationManager.AppSettings["KeePass.FlatexUuid"];
        }

        public static class Configuration
        {
            public static readonly string Path = ConfigurationManager.AppSettings["Configuration.Path"];
        }
    }
}
