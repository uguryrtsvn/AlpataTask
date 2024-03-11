
using AlpataDAL.IRepositories;
using AlpataEntities.Entities.Base;
using AutoMapper;
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
    public class BaseRepository<T> : IBaseRepository<T> where T :class, new()
    {
        private readonly AlpataDbContext db;
        protected readonly IMapper? _mapper;
        public BaseRepository(AlpataDbContext db, IMapper mapper)
        {
            _mapper = mapper;
            this.db = db;
        }

        public async Task<bool> Any(Expression<Func<T, bool>> expression)
        {
            return await db.Set<T>().AnyAsync(expression);
        }

        public async Task<bool> CreateAsync(T entity)
        { 
            await db.Set<T>().AddAsync(entity);
            return await db.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            db.Set<T>().Remove(entity);
            return await db.SaveChangesAsync() > 0;
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
 
        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? includes = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, Func<IQueryable<T>, IQueryable<T>>? selector = null, int skip = 0, int take = 0, bool ignoreQueryFilters = false)
        {
            IQueryable<T> query = db.Set<T>();
            if (ignoreQueryFilters) query = query.IgnoreQueryFilters();
            if (includes != null) query = includes(query);
            if (filter != null) query = query.Where(filter);
            if (selector != null) query = selector(query);
            if (orderBy != null) query = orderBy(query);
            if (skip > 0) query = query.Skip(skip);
            if (take > 0) query = query.Take(take);

            return await query.ToListAsync();
        }
        public async Task<List<TResult>?> GetAllAsync<TResult>(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, int skip = 0, int take = 0, bool ignoreQueryFilters = false) where TResult : class, new()
        {
            IQueryable<T> query = db.Set<T>();
            if (ignoreQueryFilters) query = query.IgnoreQueryFilters();
            if (filter != null) query = query.Where(filter);
            if (orderBy != null) query = orderBy(query);
            if (skip > 0) query = query.Skip(skip);
            if (take > 0) query = query.Take(take);
            query = query.AsNoTracking();
            var mappedQuery = _mapper?.ProjectTo<TResult>(query);
            return mappedQuery is null ? null : await mappedQuery.ToListAsync();
        }
        public async Task<bool> UpdateAsync(T entity)
        {
            db.Set<T>().Update(entity);
            return await db.SaveChangesAsync() > 0;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await db.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteRangeAsync(List<T> listEntity)
        {
            db.Set<T>().RemoveRange(listEntity); 
            return await db.SaveChangesAsync() > 0;
        }
    }
}
