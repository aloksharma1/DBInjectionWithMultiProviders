using DBInjectionWithMultiProviders.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
namespace DBInjectionWithMultiProviders.Persistence
{
    public class Repository<TEntity> : IAsyncDisposable, IRepository<TEntity> where TEntity : class, new() 
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<TEntity> dbSet;
        public Repository(IApplicationDbContext dbContext)
        {            
            _dbContext = (DbContext)dbContext;            
            this.dbSet = _dbContext.Set<TEntity>();
        }

        public TEntity Add(TEntity entity)
        {
            return dbSet.Add(entity).Entity;            
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            return (await dbSet.AddAsync(entity)).Entity;
        }

        public async Task<int> CountAsync()
        {
            return await GetAll().CountAsync();
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().CountAsync(predicate);
        }

        public void Delete(TEntity entity)
        {
            dbSet.Remove(entity);
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return dbSet.FirstOrDefault(predicate);
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            return GetAllIncluding(propertySelectors).FirstOrDefault(predicate);
        }

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await dbSet.Where(predicate).FirstOrDefaultAsync();
        }
        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            return await GetAllIncluding(propertySelectors).FirstOrDefaultAsync(predicate);
        }

        public IQueryable<TEntity> GetAll()
        {
            return GetAllIncluding();
        }

        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAllIncluding().Where(predicate);
        }

        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            return GetAllIncluding(propertySelectors).Where(predicate);
        }

        public IQueryable<TEntity> GetAll(int page, int pageCount)
        {
            return GetAll().Skip((page - 1) * pageCount).Take(pageCount);
        }

        public IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            var query = dbSet.AsQueryable();

            if (propertySelectors != null)
            {
                foreach (var propertySelector in propertySelectors)
                {
                    query = query.Include(propertySelector);
                }
            }

            return query;
        }

        public async Task<List<TEntity>> GetAllListAsync()
        {
            return await dbSet.AsQueryable().ToListAsync();
        }

        public async Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await dbSet.Where(predicate).ToListAsync();
        }

        public TEntity GetById<TId>(TId id)
        {
            return dbSet.Find(id);
        }

        public async Task<TEntity> GetByIdAsync<TId>(TId id)
        {
            return await dbSet.FindAsync(id);
        }

        public TEntity Insert(TEntity entity)
        {
            return Add(entity);
        }

        public async Task<TEntity> InsertAsync(TEntity entity)
        {
            return await AddAsync(entity);
        }

        public List<TEntity> ToList()
        {
            return dbSet.ToList();
        }

        public async Task<List<TEntity>> ToListAsync()
        {
            return await dbSet.AsQueryable().ToListAsync();
        }

        public async Task<long> LongCountAsync()
        {
            return await dbSet.AsQueryable().LongCountAsync();
        }

        public async Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await dbSet.Where(predicate).LongCountAsync();
        }

        public TEntity Update(TEntity entity)
        {
            AttachIfNot(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
            return dbSet.Update(entity).Entity;
        }

        public Task<TEntity> UpdateAsync(TEntity entity)
        {
            return Task.FromResult(Update(entity));
        }
        protected virtual void AttachIfNot(TEntity entity)
        {
            var entry = _dbContext.ChangeTracker.Entries().FirstOrDefault(ent => ent.Entity == entity);
            if (entry != null)
            {
                return;
            }

            dbSet.Attach(entity);
        }

        public List<TEntity> ToList(Expression<Func<TEntity, bool>> predicate)
        {
            return dbSet.Where(predicate).ToList();
        }

        public async Task<List<TEntity>> ToListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await dbSet.Where(predicate).ToListAsync();
        }

        public virtual IQueryable<TEntity> FindAllBy(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate != null)
            {
                return dbSet.Where(predicate);

            }
            return default;
        }
        public virtual IQueryable<TEntity> FindBy<TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> orderBy)
        {
            if (predicate != null && orderBy != null)
            {
                return FindAllBy(predicate).OrderBy(orderBy).AsQueryable<TEntity>();
            }
            return default;
        }
        public IQueryable<TEntity> Query()
        {
            return dbSet;
        }
        ValueTask IAsyncDisposable.DisposeAsync()
        {
            return _dbContext.DisposeAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }
    }
}
