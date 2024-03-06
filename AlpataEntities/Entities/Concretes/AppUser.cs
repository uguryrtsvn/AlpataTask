
using AlpataEntities.Entities.Base;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable disable
namespace AlpataEntities.Entities.Concretes
{
    public class AppUser : BaseEntity
    {
        public AppUser()
        {
            Meetings = new HashSet<Meeting>();
        }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? ImagePath { get; set; }  
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }   
        public ICollection<Meeting> Meetings { get; set; } 

    }
}
