using AlpataEntities.Entities.Concretes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace AlpataDAL.EntityConfigurations
{
    public class MeetingConfig : IEntityTypeConfiguration<Meeting>
    {
        public void Configure(EntityTypeBuilder<Meeting> builder)
        {
            builder.HasOne(m => m.CreatorUser).WithMany().HasForeignKey(m => m.CreatorUserId);
            builder.HasMany(m => m.Participants)
             .WithOne(mp => mp.Meeting)
             .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
