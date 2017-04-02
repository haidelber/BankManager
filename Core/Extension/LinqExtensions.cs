using System;
using System.Collections.Generic;
using System.Linq;

namespace BankManager.Core.Extension
{
    public static class LinqExtensions
    {
        public static decimal StdDev(this IEnumerable<decimal> values)
        {
            var enumerable = values as IList<decimal> ?? values.ToList();
            var count = enumerable.Count;
            if (count <= 1) return 0m;
            //Compute the Average
            var avg = enumerable.Average();

            //Perform the Sum of (value-avg)^2
            var sum = enumerable.Sum(d => (d - avg) * (d - avg));

            return new decimal(Math.Sqrt((double)(sum / count)));
        }
    }
}
