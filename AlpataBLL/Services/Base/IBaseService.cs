﻿using AlpataBLL.BaseResult.Abstracts;
using AlpataEntities.Dtos.MeetingDtos;
using AlpataEntities.Entities.Base; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AlpataBLL.Services.Base
{
    public interface IBaseService<T> where T : class, IBaseEntity,new()
    {
        Task<IDataResult<T>> GetAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, int skip = 0, bool ignoreQueryFilters = false);

        Task<IDataResult<TResult>> CreateAsync<TResult>(TResult dto);
        Task<IDataResult<bool>> DeleteAsync(T dto);
        Task<IDataResult<bool>> UpdateAsync(T dto);
        Task<IDataResult<List<TResult>>> GetAllAsync<TResult>(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, int skip = 0, int take = 0, bool ignoreQueryFilters = false)
           where TResult : class, new();
    }
}
