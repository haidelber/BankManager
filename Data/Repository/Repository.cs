using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BankDataDownloader.Data.Entity;

namespace BankDataDownloader.Data.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : EntityBase
    {
        private DbSet<TEntity> DbSet => DbContext.Set<TEntity>();
        public DbContext DbContext { get; }
        public IEntityEqualityComparer<TEntity> EntityEqualityComparer { get; set; }

        public Repository(DbContext dbContext)
        {
            DbContext = dbContext;
        }

        public IEnumerable<TEntity> GetAll()
        {
            return DbSet.ToList();
        }

        public TEntity GetById(object id)
        {
            return DbSet.Find(id);
        }

        public TEntity InsertOrGet(TEntity entity)
        {
            var existingEntity = Query().SingleOrDefault();
            if (existingEntity == null)
            {
                Insert(entity);
                return entity;
            }
            return existingEntity;
        }

        public TEntity InsertOrGetWithEquality(TEntity entity)
        {
            TEntity existingEntity = null;
            if (EntityEqualityComparer != null)
            {
                existingEntity = Query().SingleOrDefault(EntityEqualityComparer.Func(entity));
            }
            if (existingEntity == null)
            {
                existingEntity = GetById(entity.Id);
            }
            if (existingEntity == null)
            {
                Insert(entity);
                return entity;
            }
            return existingEntity;
        }

        public void Insert(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public void Update(TEntity entity)
        {
            DbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(TEntity entity)
        {
            if (DbContext.Entry(entity).State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }
            DbSet.Remove(entity);
        }

        public void Delete(object id)
        {
            var entityToDelete = GetById(id);
            Delete(entityToDelete);
        }

        public IQueryable<TEntity> Query()
        {
            return DbSet.AsQueryable();
        }

        public IQueryable<TEntity> QueryUnsaved()
        {
            return DbSet.Local.AsQueryable();
        }

        public void Save()
        {
            DbContext.SaveChanges();
        }
    }
}
