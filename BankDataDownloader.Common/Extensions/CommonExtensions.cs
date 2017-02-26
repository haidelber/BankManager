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
        /// <param name="originalString"></param>
        /// <returns></returns>
        public static string CleanString(this string originalString)
        {
            return Regex.Replace(originalString, @"[^\w-_]", "");
        }

        public static string CleanNumberStringFromOther(this string originalString)
        {
            return Regex.Replace(originalString, @"[^\d\.,]", "");
        }

        public static string ExtractDecimalNumberString(this string originalString)
        {
            return Regex.Match(originalString, @"\d+([\.,]\d+)?").Value;
        }
    }
}
