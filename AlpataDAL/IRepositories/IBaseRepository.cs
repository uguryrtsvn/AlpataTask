using AlpataEntities.Entities.Base;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AlpataDAL.IRepositories
{
    public interface IBaseRepository<T> where T : class, IBaseEntity, new()
    {
        Task<bool> CreateAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(T entity); 
        Task<T?> GetAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? includes = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, Func<IQueryable<T>, IQueryable<T>>? selector = null, int skip = 0, bool ignoreQueryFilters = false);


        Task<bool> Any(Expression<Func<T, bool>> expression);
        Task<T> GetWhere(Expression<Func<T, bool>> expression);
        Task<bool> AnyAsync(Expression<Func<T, bool>>? filter = null, bool ignoreQueryFilters = false);
        Task<List<T>> GetAllWhere(Expression<Func<T, bool>> expression);

    
        Task<TResult> GetFilteredFirstOrDefault<TResult>(
            Expression<Func<T, TResult>> selector,
            Expression<Func<T, bool>> expression,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null);
 
        Task<List<TResult>> GetFilteredList<TResult>(
            Expression<Func<T, TResult>> selector,
            Expression<Func<T, bool>> expression,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null,
            int limit = 0);
    }
}
