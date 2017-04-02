using System;
using BankManager.Data.Entity;

namespace BankManager.Data
{
    public interface IEntityEqualityComparer<in T> where T : EntityBase
    {
        Func<T, bool> Func(T otherEntity);
    }
}