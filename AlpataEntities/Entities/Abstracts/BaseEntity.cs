
using AlpataEntities.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlpataEntities.Entities.Abstracts
{
    public class BaseEntity : IBaseEntity
    {
        public BaseEntity()
        { 
        }
        public Guid Id { get; set; }  
        public DateTime? CreatedTime { get; set; } 
    }
}
