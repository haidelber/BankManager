using System;
using System.Runtime.InteropServices;
using System.Security;
using KeePassLib;

namespace BankDataDownloader.Core.Extension
{
    public static class KeePassExtensions
    {
        public static string ConvertToUnsecureString(this SecureString securePassword)
        {
            if (securePassword == null)
                throw new ArgumentNullException(nameof(securePassword));

            var unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        public static string ToUtf8String(this byte[] bytes )
        {
            return System.Text.Encoding.UTF8.GetString(bytes);
        }

        public static string GetTitle(this PwEntry entry)
        {
            return entry.GetString("Title");
        }

        public static string GetUserName(this PwEntry entry)
        {
            return entry.GetString("UserName");
        }

        public static string GetPassword(this PwEntry entry)
        {
            return entry.GetString("Password");
        }

        public static string GetString(this PwEntry entry, string key)
        {
            return entry.Strings?.Get(key)?.ReadUtf8()?.ToUtf8String() ?? "";
        }
    }
}