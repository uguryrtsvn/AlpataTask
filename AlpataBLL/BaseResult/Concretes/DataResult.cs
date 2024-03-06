using AlpataBLL.BaseResult.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AlpataBLL.BaseResult.Concretes
{
    public class DataResult<T> : Result, IDataResult<T>
    {
        public DataResult()
        {
                
        }
        public DataResult(T data, bool success) : base(success)
        {
            Data = data;
        }

        public DataResult(T? data, bool success, string message) : base(success, message)
        {
            Data = data;
        }
        public T? Data { get; set; }
    }
}
