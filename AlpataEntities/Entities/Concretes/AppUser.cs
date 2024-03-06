
using AlpataEntities.Entities.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
#nullable disable
namespace AlpataEntities.Entities.Concretes
{
    public class AppUser : IdentityUser<Guid>, IBaseEntity
    {
        public AppUser()
        {
            Meetings = new HashSet<Meeting>();
        }
        public string FirstName { get; set; }
        public string LastName { get; set; } 
        public string? ImagePath { get; set; } 
        public ICollection<Meeting> Meetings { get; set; }
        public DateTime? CreatedTime { get; set; } 
    }
}
