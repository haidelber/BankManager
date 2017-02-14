using System;
using BankDataDownloader.Data.Entity;

namespace BankDataDownloader.Data
{
    public interface IEntityEqualityComparer<in T> where T : EntityBase
    {
        Func<T, bool> Func(T otherEntity);
    }
}