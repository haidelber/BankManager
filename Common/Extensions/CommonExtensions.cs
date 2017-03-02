using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text.RegularExpressions;

namespace BankDataDownloader.Common.Extensions
{
    public static class CommonExtensions
    {
        public static bool SequenceEqual(this IEnumerable first, IEnumerable second)
        {
            var e1 = first.GetEnumerator();
            var e2 = second.GetEnumerator();

            while (e1.MoveNext())
            {
                if (!(e2.MoveNext() && Equals(e1.Current, e2.Current))) return false;
            }
            if (e2.MoveNext()) return false;

            return true;
        }

        public static bool DictionaryEqual<T1, T2>(this IDictionary<T1, T2> d1, IDictionary<T1, T2> d2)
        {
            return d1.Aggregate(true,
                (b, c) =>
                    b && d2.ContainsKey(c.Key) && d2[c.Key].GetType().IsArray
                        ? ((IEnumerable)d2[c.Key]).SequenceEqual((IEnumerable)c.Value)
                        : Equals(d2[c.Key], c.Value));
        }
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

        public static SecureString ConvertToSecureString(this string str)
        {
            if (str == null)
                throw new ArgumentNullException(nameof(str));

            var securePassword = new SecureString();

            foreach (char c in str)
                securePassword.AppendChar(c);

            securePassword.MakeReadOnly();
            return securePassword;
        }


        public static string ToUtf8String(this byte[] bytes)
        {
            return System.Text.Encoding.UTF8.GetString(bytes);
        }

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
