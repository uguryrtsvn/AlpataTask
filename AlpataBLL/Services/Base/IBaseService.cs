using AlpataBLL.BaseResult.Abstracts;
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
    }
}
