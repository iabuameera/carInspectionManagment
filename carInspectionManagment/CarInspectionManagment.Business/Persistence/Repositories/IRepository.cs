using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CarInspectionManagment.Contract.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace CarInspectionManagment.Business.Persistence
{
    /// <summary>
    /// Repository contract to work with an entity that implements ICorrelateBy.
    /// </summary>
    /// <typeparam name="TEntity">Entity.</typeparam>
    /// <typeparam name="TEntityId">Entity id.</typeparam>
    //public interface IRepository<TEntity, in TEntityId>
    //    where TEntity : class, new()
    //{
    //    Task<List<TEntity>> GetItemsAsync(
    //        Expression<Func<TEntity, bool>>[] filters = null,
    //        Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null,
    //        ListFilter pagingFilter = null,
    //        Type orderByType = null,
    //        string orderBy = null);

    //    Task<List<TEntity>> GetItemsAsync<TResource>(
    //        Expression<Func<TEntity, bool>>[] filters = null,
    //        Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null,
    //        ListFilter pagingFilter = null,
    //        Type orderByType = null,
    //        string orderBy = null);

    //    Task<TEntity> GetByIdAsync(
    //        TEntityId id,
    //        Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null);

    //    Task<TEntity> FirstOrDefaultAsync(
    //        Expression<Func<TEntity, bool>>[] filters = null,
    //        Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null);

    //    Task<TEntity> LastOrDefaultAsync(
    //        Expression<Func<TEntity, bool>>[] filters = null,
    //        Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null,
    //        Expression<Func<TEntity, object>> orderByDescendingColumnSelector = null);

    //    Task<int> CountAsync(
    //        params Expression<Func<TEntity, bool>>[] filters);

    //    Task<bool> ExistsAsync(
    //        params Expression<Func<TEntity, bool>>[] filters);

    //    Task<bool> ExistsAsync(
    //        Expression<Func<TEntity, bool>>[] filters,
    //        Func<IQueryable<TEntity>, IQueryable<TEntity>> includes);

    //    Task SaveAsync();

    //    void Delete(
    //        TEntity entity);

    //    void Update(
    //        TEntity entity,
    //        string modifiedBy = null,
    //        DateTimeOffset? modifiedOn = null);

    //    void Add(
    //        TEntity entity,
    //        string createdBy = null,
    //        DateTimeOffset? createdOn = null);

    //    IDbContextTransaction BeginTransaction();

    //    void CommitTransaction();

    //    void RollBackTransaction();

    //    Task<IDbContextTransaction> BeginTransactionAsync();

    //    void MarkUnchanged(
    //        TEntity entity);

    //    void MarkModified(
    //        TEntity entity);

    //    void MarkModified<TSubEntity>(
    //        TSubEntity entity)
    //        where TSubEntity : class;

    //    void MarkUnchanged<TSubEntity>(
    //        TSubEntity entity)
    //        where TSubEntity : class;

    //    void MarkDeleted<TSubEntity>(
    //        TSubEntity entity)
    //        where TSubEntity : class;

    //    void RemoveRange(
    //        Func<TEntity, bool> predicate);
    //}

    public interface IRepository<TEntity>
        where TEntity : class, new()
    {
        Task<List<TEntity>> GetItemsAsync(
            Expression<Func<TEntity, bool>>[] filters = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null,
            ListFilter pagingFilter = null,
            Type orderByType = null,
            string orderBy = null);

        Task<List<TEntity>> GetItemsAsync<TResource>(
            Expression<Func<TEntity, bool>>[] filters = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null,
            ListFilter pagingFilter = null,
            Type orderByType = null,
            string orderBy = null);

        Task<TEntity> FirstOrDefaultAsync(
            Expression<Func<TEntity, bool>>[] filters = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null);

        Task<int> CountAsync(
            params Expression<Func<TEntity, bool>>[] filters);

        Task<bool> ExistsAsync(params Expression<Func<TEntity, bool>>[] filters);

        Task<bool> ExistsAsync(
            Expression<Func<TEntity, bool>>[] filters,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> includes);

        Task SaveAsync();

        void Delete(
            TEntity entity);

        void Update(
            TEntity entity,
            string modifiedBy = null,
            DateTimeOffset? modifiedOn = null);

        void Add(
            TEntity entity,
            string createdBy = null,
            DateTimeOffset? createdOn = null);

        void RemoveRange(
            Func<TEntity, bool> predicate);
    }
}
