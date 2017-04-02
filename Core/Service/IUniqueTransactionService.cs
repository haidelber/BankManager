using System.Collections.Generic;
using BankDataDownloader.Data.Entity;

namespace BankDataDownloader.Core.Service
{
    public interface IUniqueTransactionService
    {
        IEnumerable<BankTransactionEntity> AddAvailabilityDateBasedUniqueId(
            IEnumerable<BankTransactionEntity> input);

        IEnumerable<BankTransactionEntity> AddPostingDateBasedUniqueId(
            IEnumerable<BankTransactionEntity> input);
    }
}