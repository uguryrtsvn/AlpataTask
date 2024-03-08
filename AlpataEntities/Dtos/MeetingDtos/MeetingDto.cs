using AlpataEntities.Entities.Concretes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlpataEntities.Dtos.MeetingDtos
{
    public class MeetingDto
    {
        public MeetingDto()
        {
            Inventories = new List<Inventory>();
        }
        public Guid Id { get; set; }
        public Guid CreatorUserId { get; set; }
        public AppUser CreatorUser { get; set; }
        public bool isActive { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime Description { get; set; }
        public List<Inventory>? Inventories { get; set; }
        public List<AppUser>? Participants { get; set; }
    }
}
