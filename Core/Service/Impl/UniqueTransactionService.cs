using System;
using System.Collections.Generic;
using System.Linq;
using BankDataDownloader.Data.Entity;

namespace BankDataDownloader.Core.Service.Impl
{
    public class UniqueTransactionService : IUniqueTransactionService {
        public IEnumerable<BankTransactionEntity> AddAvailabilityDateBasedUniqueId(
            IEnumerable<BankTransactionEntity> input)
        {
            var grouping = input.GroupBy(entity => entity.AvailabilityDate.Date);
            return AddUniqueId(grouping);
        }

        public IEnumerable<BankTransactionEntity> AddPostingDateBasedUniqueId(
            IEnumerable<BankTransactionEntity> input)
        {
            var grouping = input.GroupBy(entity => entity.PostingDate.Date);
            return AddUniqueId(grouping);
        }

        private static IEnumerable<BankTransactionEntity> AddUniqueId(IEnumerable<IGrouping<DateTime,BankTransactionEntity>> grouping)
        {
            foreach (var group in grouping)
            {
                var uniqueId = 1;
                foreach (var bankTransactionEntity in group)
                {
                    bankTransactionEntity.UniqueId = uniqueId++;
                    yield return bankTransactionEntity;
                }
            }
        }
    }
}