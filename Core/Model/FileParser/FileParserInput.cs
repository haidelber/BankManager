using System;
using System.Collections.Generic;
using BankManager.Core.Parser;
using BankManager.Data.Entity;

namespace BankManager.Core.Model.FileParser
{
    public class FileParserInput
    {
        public string FilePath { get; set; }
        public Type TargetEntity { get; set; }
        public IFileParser FileParser { get; set; }
        /// <summary>
        /// The <see cref="AccountEntity"/> or <see cref="PortfolioEntity"/> owning the <see cref="TargetEntity"/>.
        /// </summary>
        public object OwningEntity { get; set; }
        public Func<object, object> UniqueIdGroupingFunc { get; set; }
        public List<Func<object, object>> OrderingFuncs { get; set; }
        public decimal Balance { get; set; }
        public Func<decimal> BalanceSelectorFunc { get; set; }
    }
}