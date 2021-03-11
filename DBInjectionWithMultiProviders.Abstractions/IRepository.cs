using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DBInjectionWithMultiProviders.Abstractions
{
    /// <summary>
    /// TEntity class will always be BaseEntity or Its Inherited Child Classes
    /// </summary>
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Find Unique Records By Defined Id Type 
        /// </summary>
        /// <typeparam name="T">BaseEntity or Its Inherited Child Classes</typeparam>
        /// <param name="id">unique id field of db records</param>
        /// <returns>Defined Return Types</returns>
        TEntity GetById<TId>(TId id);

        /// <summary>
        /// Asynchronously Find Unique Records By Defined Id Type 
        /// </summary>
        /// <typeparam name="T">BaseEntity or Its Inherited Child Classes</typeparam>
        /// <param name="id">unique id field of db records</param>
        /// <returns>Defined Return Types</returns>
        Task<TEntity> GetByIdAsync<TId>(TId id);

        /// <summary>
        /// Fetch LisTEntity Of Records Of Type T
        /// </summary>
        /// <typeparam name="T">BaseEntity or Its Inherited Child Classes</typeparam>
        /// <returns>Defined List TEntity Return Types</returns>
        List<TEntity> ToList();

        /// <summary>
        /// The to list.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>The List<TEntity>.</returns>
        List<TEntity> ToList(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Asynchronously Fetch List TEntity Of Records Of Type T
        /// </summary>
        /// <typeparam name="T">BaseEntity or Its Inherited Child Classes</typeparam>
        /// <returns>Defined List TEntity Return Types</returns>
        Task<List<TEntity>> ToListAsync();

        /// <summary>
        /// The to list async.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>The result.</returns>
        Task<List<TEntity>> ToListAsync(Expression<Func<TEntity, bool>> predicate);
        /// <summary>
        /// Add Data To Passed Entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        TEntity Add(TEntity entity);

        /// <summary>
        /// Add Records To Passed Entity
        /// </summary>
        /// <typeparam name="T">BaseEntity or Its Inherited Child Classes</typeparam>
        /// <param name="entity">DbContexTEntity Class</param>
        /// <returns>Added Entity</returns>
        Task<TEntity> AddAsync(TEntity entity);

        /// <summary>
        /// The count async.
        /// </summary>
        /// <returns>The result.</returns>
        Task<int> CountAsync();

        /// <summary>
        /// The count async.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>The result.</returns>
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// The long count async.
        /// </summary>
        /// <returns>The result.</returns>
        Task<long> LongCountAsync();

        /// <summary>
        /// The long count async.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>The result.</returns>
        Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// The first or default.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>The result.</returns>
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// The first or default async.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>The result.</returns>
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// The first or default.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="propertySelectors">The property selectors.</param>
        /// <returns>The result.</returns>
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] propertySelectors);

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>The result.</returns>
        IQueryable<TEntity> GetAll();

        /// <summary>
        /// The get all.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>The result.</returns>
        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// The get all including.
        /// </summary>
        /// <param name="propertySelectors">The property selectors.</param>
        /// <returns>The result.</returns>
        IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] propertySelectors);

        /// <summary>
        /// The get all.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="propertySelectors">The property selectors.</param>
        /// <returns>The result.</returns>
        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] propertySelectors);

        /// <summary>
        /// The get all list async.
        /// </summary>
        /// <returns>The result.</returns>
        Task<List<TEntity>> GetAllListAsync();

        /// <summary>
        /// The get all list async.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>The result.</returns>
        Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// The get all.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="pageCount">The page count.</param>
        /// <returns>The result.</returns>
        IQueryable<TEntity> GetAll(int page, int pageCount);

        /// <summary>
        /// The query.
        /// </summary>
        /// <returns>The result.</returns>
        IQueryable<TEntity> Query();

        /// <summary>
        /// The insert.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The result.</returns>
        TEntity Insert(TEntity entity);

        /// <summary>
        /// The insert async.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The result.</returns>
        Task<TEntity> InsertAsync(TEntity entity);

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The result.</returns>
        TEntity Update(TEntity entity);

        /// <summary>
        /// The update async.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The result.</returns>
        Task<TEntity> UpdateAsync(TEntity entity);

        /// <summary>
        /// The find all by.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>The result.</returns>
        IQueryable<TEntity> FindAllBy(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// The find by.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="orderBy">The order by.</param>
        /// <returns>The result.</returns>
        IQueryable<TEntity> FindBy<TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> orderBy);

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Delete(TEntity entity);
        Task<int> SaveChangesAsync();
    }
}
