using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankDataDownloader.Core.Extension
{
    public static class CommonExtensions
    {
        public static string ToSortableFileName(this DateTime dateTime)
        {
            return dateTime.ToString("yyyyMMddHHmmss");
        }
    }
}
