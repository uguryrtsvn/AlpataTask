using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlpataEntities.Entities.Concretes;
using System.Reflection.Emit;

namespace AlpataDAL.EntityConfigurations
{
    public class AppUserConfig : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.HasMany(u => u.Participants)
         .WithOne(mp => mp.AppUser)
         .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
