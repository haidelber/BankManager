using System.Collections.Generic;
using System.Linq;
using BankDataDownloader.Data.Entity;

namespace BankDataDownloader.Data.Repository
{
    public interface IRepository<TEntity> where TEntity : EntityBase
    {
        IEnumerable<TEntity> GetAll();
        TEntity GetById(long id);
        IQueryable<TEntity> Query();
        void Save();
        void Delete(TEntity entity);
        void Delete(long id);
        TEntity Insert(TEntity entity);
        void Update(TEntity entity);
        TEntity InsertOrGet(TEntity entity);
        IQueryable<TEntity> QueryUnsaved();
        TEntity InsertOrGetWithEquality(TEntity entity);
    }
}