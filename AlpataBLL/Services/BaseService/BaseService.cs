﻿using AlpataBLL.BaseResult.Abstracts;
using AlpataBLL.BaseResult.Concretes;
using AlpataBLL.Constants;
using AlpataDAL.IRepositories;
using AlpataEntities.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AlpataBLL.Services.BaseService
{
    public abstract class BaseService<T> : IBaseService<T> where T : class, IBaseEntity, new()
    {
        protected readonly IBaseRepository<T> _entityRepository;
        public BaseService(IBaseRepository<T> entityRepository)
        {
            _entityRepository = entityRepository;
        }

        public virtual async Task<IDataResult<TResult>> GetAsync<TResult>(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, int skip = 0, bool ignoreQueryFilters = false)
              where TResult : class, new()
        {
            TResult? getResult = await _entityRepository.GetAsync<TResult>(filter, orderBy, skip, ignoreQueryFilters);
            return getResult != null ?
                new SuccessDataResult<TResult>(getResult, Messages.Found) :
                new ErrorDataResult<TResult>(Messages.NotFound);
        }
    }
}