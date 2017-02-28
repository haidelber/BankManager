using System.Security;
using BankDataDownloader.Common.Extensions;

namespace BankDataDownloader.Common
{
    public static class TestConstants
    {
        public const string TestDataPath = @"C:\git\testdata";
        public static class Service
        {
            public static class KeePass
            {
                public static readonly string Path = System.IO.Path.Combine(TestConstants.TestDataPath, @"KeePassTest.kdbx");
                public static readonly SecureString Password = "AjEhyc7SqUkz,5a4Imu5".ConvertToSecureString();

                public const string RaiffeisenUuid = "C950397EFB46704BA8AC47C474C972D2";
                public const string DkbUuid = "96168F79F3E7D34BB0159FC67AA7B0D8";
                public const string Number26Uuid = "8BFFFD0A2E314B4E92B6ADDA321616D3";
                public const string RciUuid = "A24CCF49ACFB574F91D061B8113BB259";
                public const string SantanderUuid = "EEC9DCD89EF0C9419997776FC900387A";
                public const string PayPalUuid = "37381D434802A34EA935592FCC99E98D";
            }

            public static class Configuration
            {
                public static readonly string Path = System.IO.Path.Combine(TestConstants.TestDataPath, @"BankDataDownloader.config");
            }
        }

        public static class DownloadHandler
        {
            public static readonly string RaiffeisenPath = System.IO.Path.Combine(TestConstants.TestDataPath, @"Raiffeisen");
            public static readonly string DkbPath = System.IO.Path.Combine(TestConstants.TestDataPath, @"DKB");
            public static readonly string Number26Path = System.IO.Path.Combine(TestConstants.TestDataPath, @"Number26");
            public static readonly string RciPath = System.IO.Path.Combine(TestConstants.TestDataPath, @"Rci");
            public static readonly string SantanderPath = System.IO.Path.Combine(TestConstants.TestDataPath, @"Santander");
            public static readonly string PayPalPath = System.IO.Path.Combine(TestConstants.TestDataPath, @"PayPal");
        }

        public static class Parser
        {
            public static class CsvParser
            {
                public static readonly string RaiffeisenPath = System.IO.Path.Combine(TestConstants.TestDataPath, @"Raiffeisen.csv");
                public static readonly string Number26Path = System.IO.Path.Combine(TestConstants.TestDataPath, @"Number26.csv");
                public static readonly string PayPalPath = System.IO.Path.Combine(TestConstants.TestDataPath, @"PayPal.csv");
                public static readonly string DkbGiroPath = System.IO.Path.Combine(TestConstants.TestDataPath, @"DkbGiro.csv");
                public static readonly string DkbCreditPath = System.IO.Path.Combine(TestConstants.TestDataPath, @"DkbCredit.csv");
            }
        }

        public static class Data
        {
            public static readonly string DatabasePath = System.IO.Path.Combine(TestConstants.TestDataPath, @"sqlite.db");
        }
    }
}
