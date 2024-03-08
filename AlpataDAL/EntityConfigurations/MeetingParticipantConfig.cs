using AlpataEntities.Entities.Concretes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlpataDAL.EntityConfigurations
{
    public class MeetingParticipantConfig : IEntityTypeConfiguration<MeetingParticipant>
    {
        public void Configure(EntityTypeBuilder<MeetingParticipant> builder)
        {
            builder.HasKey(um => new { um.AppUserId, um.MeetingId });

            builder.HasOne(um => um.AppUser)
                .WithMany(u => u.Participants)
                .HasForeignKey(um => um.AppUserId);

            builder.HasOne(um => um.Meeting)
                .WithMany(m => m.Participants)
                .HasForeignKey(um => um.MeetingId); 
        }
    }
}
