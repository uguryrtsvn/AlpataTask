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
            Inventories = new();
            Participants = new();
        }
        public Guid Id { get; set; }
        public Guid? CreatorUserId { get; set; }
        public AppUser? CreatorUser { get; set; }
        public string? Name { get; set; }
        public bool isActive { get; set; } = true;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime CreatedTime { get; set; }
        public string? Description { get; set; } 
        public virtual List<Inventory> Inventories { get; set; }
        public List<MeetingParticipant> Participants { get; set; }
    }
}
