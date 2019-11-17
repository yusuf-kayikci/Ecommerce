using Ecommerce.Common.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using X.PagedList;

namespace Ecommerce.Common.Core
{
    class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        internal EcommerceContext context;
        internal DbSet<TEntity> dbSet;

        public BaseRepository(EcommerceContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> GetWithRawSql(string query,
            params object[] parameters)
        {
            return dbSet.FromSql(query, parameters).ToList();
        }

        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public virtual TEntity GetByID(object id)
        {
            return dbSet.Find(id);
        }

        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
        }


        public virtual IEnumerable<TEntity> IncludeMultiple(Expression<Func<TEntity, object>>[] includes, Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> entities = dbSet;
            if (filter != null)
            {
                entities = entities.Where(filter);
            }
            foreach (var item in includes)
            {
                entities = entities.Include(item);
            }
            return entities;
        }

        public virtual PaginationModel<TEntity> GetWithPagination(int pageIndex, int pageSize = 10, Expression<Func<TEntity, bool>> filter = null)
        {
            var pageNumber = (pageIndex < 1) ? 0 : pageIndex - 1; 
            IQueryable<TEntity> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            var totalItemCount = query.Count();
            var list = query.Skip(pageNumber * pageSize).Take(pageSize);
            return new PaginationModel<TEntity>
            {
                Items = list,
                PageNumber = pageNumber + 1,
                PageSize = pageSize,
                TotalItemCount = totalItemCount
            }; 
        }

    }

}

