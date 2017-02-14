using System;
using System.Text.RegularExpressions;

namespace BankDataDownloader.Common.Extensions
{
    public static class CommonExtensions
    {
        public static string ToSortableFileName(this DateTime dateTime)
        {
            return dateTime.ToString("yyyyMMddHHmmss");
        }

        /// <summary>
        /// Only allows digits, letters, -, _
        /// </summary>
        /// <param name="originalIban"></param>
        /// <returns></returns>
        public static string CleanString(this string originalIban)
        {
            return Regex.Replace(originalIban, @"[^\w-_]", "");
        }
    }
}
