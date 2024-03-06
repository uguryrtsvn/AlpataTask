
using AlpataEntities.Entities.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlpataEntities.Entities.Concretes
{
    public class Meeting: BaseEntity
    {
        public Meeting()
        {
           Inventories = new HashSet<Inventory>();
        }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime Description { get; set; }
        public ICollection<Inventory> Inventories { get; set; }
    }
}
