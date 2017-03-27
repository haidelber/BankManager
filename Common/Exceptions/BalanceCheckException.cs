using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankDataDownloader.Common.Exceptions
{
    public class BalanceCheckException : Exception
    {
        public decimal Expected { get; }
        public decimal Actual { get; }
        public BalanceCheckException(decimal expected, decimal actual)
        {
            Expected = expected;
            Actual = actual;
        }

        public BalanceCheckException(decimal expected, decimal actual, string message) : base(message)
        {
            Expected = expected;
            Actual = actual;
        }

        public BalanceCheckException(decimal expected, decimal actual, string message, Exception innerException) : base(message, innerException)
        {
            Expected = expected;
            Actual = actual;
        }
    }
}
