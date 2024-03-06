
using AlpataDAL.IRepositories;
using AlpataEntities.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AlpataDAL.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class, IBaseEntity,new ()
    {
        private readonly AlpataDbContext db;

        public BaseRepository(AlpataDbContext db)
        {
            this.db = db;
        }

        public async Task<bool> Any(Expression<Func<T, bool>> expression)
        {
            return await db.Set<T>().AnyAsync(expression);
        }

        public async Task<bool> CreateAsync(T entity)
        {
            entity.CreatedTime = DateTime.Now;
            await db.Set<T>().AddAsync(entity);
            return await db.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(T entity)
        { 
            db.Set<T>().Remove(entity);
            return await db.SaveChangesAsync() > 0;
        }
        public async Task<List<T>> GetAllWhere(Expression<Func<T, bool>> expression)
        {
            return await db.Set<T>().Where(expression).ToListAsync();
        }
        public async Task<T?> GetAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? includes = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, Func<IQueryable<T>, IQueryable<T>>? selector = null, int skip = 0, bool ignoreQueryFilters = false)
        {
            IQueryable<T> query = db.Set<T>();
            if (ignoreQueryFilters) query = query.IgnoreQueryFilters();
            if (includes != null) query = includes(query);
            if (filter != null) query = query.Where(filter);
            if (selector != null) query = selector(query);
            if (orderBy != null) query = orderBy(query);
            if (skip > 0) query = query.Skip(skip);
            return await query.FirstOrDefaultAsync();
        }
        public async Task<bool> AnyAsync(Expression<Func<T, bool>>? filter = null, bool ignoreQueryFilters = false)
        {
            IQueryable<T> query = db.Set<T>();
            if (ignoreQueryFilters) query = query.IgnoreQueryFilters();
            if (filter != null) query = query.Where(filter);
            return await query.AnyAsync();
        }
        public async Task<TResult> GetFilteredFirstOrDefault<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> expression, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null)
        {
            IQueryable<T> query = db.Set<T>();
            if (includes != null) query = includes(query);
            if (expression != null) query = query.Where(expression);
            if (orderBy != null) return await orderBy(query).Select(selector).FirstOrDefaultAsync();
            else return await query.Select(selector).FirstOrDefaultAsync();
        }

        public async Task<List<TResult>> GetFilteredList<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> expression, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null, int limit = 0)
        {
            IQueryable<T> query = db.Set<T>();
            if (includes != null) query = includes(query);
            if (expression != null) query = query.Where(expression);
            if (limit > 0) query = query.Take(limit);
            if (orderBy != null) return await orderBy(query).Select(selector).ToListAsync();
            else return await query.Select(selector).ToListAsync();
        }

        public async Task<T> GetWhere(Expression<Func<T, bool>> expression)
        {
            return await db.Set<T>().FirstOrDefaultAsync(expression);
        }

        public async Task<bool> UpdateAsync(T entity)
        { 
            db.Set<T>().Update(entity);
            return await db.SaveChangesAsync() > 0;
        }
         
    }
}
