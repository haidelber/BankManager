using BankManager.Common.Extensions;
using KeePassLib;

namespace BankManager.Core.Extension
{
    public static class KeePassExtensions
    {
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