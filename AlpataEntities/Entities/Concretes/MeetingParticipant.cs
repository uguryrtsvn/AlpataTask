using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlpataEntities.Entities.Concretes
{
    public class MeetingParticipant
    { 
        public Guid AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public Guid MeetingId { get; set; }
        public Meeting Meeting { get; set; }
    }
}
