
using AlpataEntities.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlpataEntities.Entities.Concretes
{
    public class Meeting : BaseEntity
    {
        public Meeting()
        {
            Inventories = new HashSet<Inventory>();
            Participants = new HashSet<AppUser>();
        }
        public string Name { get; set; }

        public Guid CreatorUserId { get; set; }
        public AppUser CreatorUser { get; set; }
        public bool isActive { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime Description { get; set; }
        public ICollection<Inventory>? Inventories { get; set; } 
        public ICollection<AppUser>? Participants { get; set; }
    }
}
