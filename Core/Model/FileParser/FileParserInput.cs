using System;
using BankDataDownloader.Core.Parser;
using BankDataDownloader.Data.Entity;

namespace BankDataDownloader.Core.Model.FileParser
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
        public decimal Balance { get; set; }
        public Func<decimal> BalanceSelectorFunc { get; set; }
    }
}