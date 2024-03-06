using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlpataEntities.Entities.Base
{
    public interface IBaseEntity
    {
        public Guid Id { get; set; }
        public DateTime? CreatedTime { get; set; }
    }
}
