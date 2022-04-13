using CarInspectionManagment.Business.Persistence.Entities;
using CarInspectionManagment.Contract.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CarInspectionManagment.Business.Persistence.Repositories
{
    /// <summary>
    /// Repository that works with an entity that implements ICorrelateBy.
    /// </summary>
    /// <typeparam name="TEntity">Entity.</typeparam>
    /// <typeparam name="TEntityId">Entity id.</typeparam>
    //public class Repository<TEntity, TEntityId> : Repository<TEntity>,
    //    IRepository<TEntity, TEntityId>
    //    where TEntity : class, ICorrelateBy<TEntityId>,  new()
    //{
    //    public Repository(
    //        CarInspectionManagementContext context )
    //        : base(context)
    //    {
    //    }

    //    public virtual async Task<TEntity> GetByIdAsync(
    //        TEntityId id,
    //        Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
    //    {


    //        return await GetQueryable()
    //              .ApplyIncludes(includes)
    //              .FirstOrDefaultAsync(e => e.Id.Equals(id));
    //    }

    //    public IDbContextTransaction BeginTransaction()
    //    {
    //        return Context.Database.BeginTransaction();
    //    }

    //    public virtual void CommitTransaction()
    //    {
    //        Context.Database.CommitTransaction();
    //    }

    //    public virtual void RollBackTransaction()
    //    {
    //        Context.Database.RollbackTransaction();
    //    }

    //    public Task<IDbContextTransaction> BeginTransactionAsync()
    //    {
    //        return Context.Database.BeginTransactionAsync();
    //    }

    //    public void MarkUnchanged(
    //        TEntity entity)
    //    {
    //        Context.Entry(entity).State = EntityState.Unchanged;
    //    }

    //    public void MarkModified(
    //        TEntity entity)
    //    {
    //        Context.Entry(entity).State = EntityState.Modified;
    //    }

    //    public void MarkDeleted<TSubEntity>(
    //        TSubEntity entity)
    //        where TSubEntity : class
    //    {
    //        Context.Entry(entity).State = EntityState.Deleted;
    //    }

    //    public void MarkUnchanged<TSubEntity>(
    //        TSubEntity entity)
    //        where TSubEntity : class
    //    {
    //        Context.Entry(entity).State = EntityState.Unchanged;
    //    }

    //    public void MarkModified<TSubEntity>(
    //        TSubEntity entity)
    //        where TSubEntity : class
    //    {
    //        Context.Entry(entity).State = EntityState.Modified;
    //    }
    //}

    /// <summary>
    /// Repository that works with an entity.
    /// </summary>
    /// <typeparam name="TEntity">Entity.</typeparam>
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class, new()
    {
        public Repository(
            CarInspectionManagementContext context
            )
        {
            Context = context;
        }



        protected virtual CarInspectionManagementContext Context { get; private set; }

        protected virtual IQueryable<TEntity> GetQueryable()
        {
            return Context.Set<TEntity>().AsNoTracking();
        }

        public virtual async Task<List<TEntity>> GetItemsAsync<TResource>(
            Expression<Func<TEntity, bool>>[] filters = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null,
            ListFilter pagingFilter = null,
            Type orderByType = null,
            string orderBy = null)
        {
            return await GetQueryable()
                .ApplyFilters(filters)
                .ApplyIncludes(includes)
                .ApplySorting<TEntity, TResource>(pagingFilter)
                .ApplyPaging(pagingFilter)
                .ToListAsync();
        }

        public virtual async Task<List<TEntity>> GetItemsAsync(
            Expression<Func<TEntity, bool>>[] filters = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null,
            ListFilter pagingFilter = null,
            Type orderByType = null,
            string orderBy = null)
        {
            var query = GetQueryable();
            if (orderByType != null &&
                 !string.IsNullOrWhiteSpace(orderBy))
            {
                query = query.OrderBy(orderByType, orderBy);
            }
            return await query
                .ApplyFilters(filters)
                .ApplyIncludes(includes)
                .ApplyPaging(pagingFilter)
                .ToListAsync();
        }

        public virtual async Task<TEntity> FirstOrDefaultAsync(
            Expression<Func<TEntity, bool>>[] filters = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            return await GetQueryable()
                .ApplyFilters(filters)
                .ApplyIncludes(includes)
                .FirstOrDefaultAsync();
        }

        public virtual async Task<TEntity> LastOrDefaultAsync(
            Expression<Func<TEntity, bool>>[] filters = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null,
            Expression<Func<TEntity, object>> orderByDescendingColumnSelector = null)
        {
            if (orderByDescendingColumnSelector == null)
            {
                orderByDescendingColumnSelector = e => e.GetPropValue("Id");
            }

            return await GetQueryable()
                .ApplyFilters(filters)
                .ApplyIncludes(includes)
                .OrderByDescending(orderByDescendingColumnSelector)
                .FirstOrDefaultAsync();
        }

        public virtual async Task<int> CountAsync(
            params Expression<Func<TEntity, bool>>[] filters)
        {
            return await GetQueryable()
                .ApplyFilters(filters)
                .CountAsync();
        }

        public virtual async Task<bool> ExistsAsync(
            params Expression<Func<TEntity, bool>>[] filters)
        {
            return await GetQueryable()
                .ApplyFilters(filters)
                .AnyAsync();
        }

        public virtual async Task<bool> ExistsAsync(
            Expression<Func<TEntity, bool>>[] filters,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> includes)
        {
            return await GetQueryable()
                .ApplyIncludes(includes)
                .ApplyFilters(filters)
                .AnyAsync();
        }

        public async Task SaveAsync()
        {
            await Context.SaveChangesAsync();
        }

        public void Update(
            TEntity entity,
            string modifiedBy = null,
            DateTimeOffset? modifiedOn = null)
        {
            //if (modifiedBy != null &&
            //    modifiedOn != null &&
            //    entity is TrackableEntity tEntity)
            //{
            //    tEntity.ModifiedBy = modifiedBy;
            //    tEntity.ModifiedOn = modifiedOn;
            //}
            //else if (entity is TrackableEntity trackableEntity)
            //{
            //    trackableEntity.ModifiedBy = UserContext.UserName;
            //    trackableEntity.ModifiedOn = DateTime.UtcNow;
            //}

            Context.Set<TEntity>().Update(entity);
        }

        public void Add(
            TEntity entity,
            string createdBy = null,
            DateTimeOffset? createdOn = null)
        {
            //if (createdOn != null &&
            //    createdBy != null &&
            //    entity is ITrackableEntity)
            //{
            //    var tEntity = (ITrackableEntity)entity;
            //    tEntity.CreatedBy = createdBy;
            //    tEntity.CreatedOn = createdOn.Value;
            //    tEntity.ModifiedBy = createdBy;
            //    tEntity.ModifiedOn = createdOn;
            //}
            //else if (entity is ITrackableEntity trackableEntity)
            //{
            //    var currentDate = DateTime.UtcNow;
            //    trackableEntity.CreatedBy = UserContext.UserName;
            //    trackableEntity.CreatedOn = currentDate;
            //    trackableEntity.ModifiedBy = UserContext.UserName;
            //    trackableEntity.ModifiedOn = currentDate;
            //}

            Context.Set<TEntity>().Add(entity);
        }

        public void Delete(
            TEntity entity)
        {
            //if (entity is ITrackableEntity trackable)
            //{
            //    trackable.ModifiedBy = UserContext.UserName;
            //    trackable.ModifiedOn = DateTime.UtcNow;
            //}

            //if (entity is ISoftDeletable deletable)
            //{
            //    deletable.IsDeleted = true;
            //    Context.Set<TEntity>().Update(entity);
            //}
            //else
                Context.Set<TEntity>().Remove(entity);
        }

        public void RemoveRange(
            Func<TEntity, bool> predicate)
        {
            Context.Set<TEntity>()
                .RemoveRange
                (Context.Set<TEntity>()
                    .Where(predicate));
        }
    }
}
