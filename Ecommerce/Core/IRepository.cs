using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

public interface IRepository<TEntity> where TEntity : class
{
    IEnumerable<TEntity> Get(
        Expression<Func<TEntity, bool>> filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
    TEntity GetByID(object id);
    IEnumerable<TEntity> GetWithRawSql(string query,
        params object[] parameters);
    void Insert(TEntity entity);
    void Update(TEntity entityToUpdate);
    void Delete(TEntity entityToDelete);
    void Delete(object id);
    IEnumerable<TEntity> IncludeMultiple( Expression<Func<TEntity, object>>[] includes , Expression<Func<TEntity , bool>> filter=null);
    Tuple<IEnumerable<TEntity> , int> GetWithPagination(int pageNumber, int pageSize = 10, Expression<Func<TEntity, bool>> filter = null);
}
