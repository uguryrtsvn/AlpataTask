using AlpataBLL.BaseResult.Abstracts;
using AlpataBLL.BaseResult.Concretes;
using AlpataBLL.Constants;
using AlpataDAL.IRepositories;
using AlpataDAL.Repositories; 
using AlpataEntities.Entities.Base;
using AlpataEntities.Entities.Concretes;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AlpataBLL.Services.Base
{
    public abstract class BaseService<T> : IBaseService<T> where T : class, IBaseEntity, new()
    {
        public readonly IMapper _mapper;
         
        protected readonly IBaseRepository<T> _entityRepository;
        public BaseService(IBaseRepository<T> entityRepository, IMapper mapper)
        {
            _entityRepository = entityRepository;
            _mapper = mapper;
        } 

        public async Task<IDataResult<TResult>> CreateAsync<TResult>(TResult dto)
        {
            var result = await _entityRepository.CreateAsync(_mapper.Map<T>(dto));
            return result ? new SuccessDataResult<TResult>(dto) : new ErrorDataResult<TResult>(dto);
        }

        public virtual async Task<IDataResult<T>> GetAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, int skip = 0, bool ignoreQueryFilters = false) 
        {
            T? getResult = await _entityRepository.GetAsync(filter,null, orderBy,null, skip, ignoreQueryFilters);
            return getResult != null ?
                new SuccessDataResult<T>(getResult, Messages.Found) :
                new ErrorDataResult<T>(Messages.NotFound);
        }
        public virtual async Task<IDataResult<List<TResult>>> GetAllAsync<TResult>(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, int skip = 0, int take = 0, bool ignoreQueryFilters = false)
           where TResult : class, new()
        {
            List<TResult>? getAllResult = await _entityRepository.GetAllAsync<TResult>(filter, orderBy, skip, take, ignoreQueryFilters);
            return getAllResult != null ?
                new SuccessDataResult<List<TResult>>(getAllResult, Messages.ListSuccess) :
                new ErrorDataResult<List<TResult>>(Messages.ListFailed);
        }
    }
}
