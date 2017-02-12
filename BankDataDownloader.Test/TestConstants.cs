using System.Security;
using BankDataDownloader.Core.Extension;

namespace DataDownloader.Test
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
            }
            
        }

        public static class Data
        {
            public static readonly string DatabasePath = System.IO.Path.Combine(TestConstants.TestDataPath, @"sqlite.db");
        }
    }
}
