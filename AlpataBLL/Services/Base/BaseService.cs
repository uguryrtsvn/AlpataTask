using AlpataBLL.BaseResult.Abstracts;
using AlpataBLL.BaseResult.Concretes;
using AlpataBLL.Constants;
using AlpataDAL.IRepositories;
using AlpataEntities.Entities.Base;
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
        protected readonly IBaseRepository<T> _entityRepository;
        public BaseService(IBaseRepository<T> entityRepository)
        {
            _entityRepository = entityRepository;
        }

        public virtual async Task<IDataResult<T>> GetAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, int skip = 0, bool ignoreQueryFilters = false) 
        {
            T? getResult = await _entityRepository.GetAsync(filter,null, orderBy,null, skip, ignoreQueryFilters);
            return getResult != null ?
                new SuccessDataResult<T>(getResult, Messages.Found) :
                new ErrorDataResult<T>(Messages.NotFound);
        }

    }
}
