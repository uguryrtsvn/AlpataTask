using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlpataBLL.BaseResult.Abstracts
{
    public interface IDataResult<T> :IBaseResult
    {
        T? Data { get; set; }
    }
}
