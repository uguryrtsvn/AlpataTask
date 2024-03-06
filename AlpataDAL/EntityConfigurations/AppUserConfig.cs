using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlpataEntities.Entities.Concretes;

namespace AlpataDAL.EntityConfigurations
{
    public class AppUserConfig : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
           //builder.HasMany(x => x.Meetings).WithOne(x => x.Id).HasForeignKey<Company>(c => c.CreatorUserId);

            //builder.HasMany(x => x.Articles).WithOne(x => x.ModifierUser).HasForeignKey(x => x.ModifierUserId);
            //builder.HasMany(x => x.Articles).WithOne(x => x.DeletoryUser).HasForeignKey(x => x.DeletoryUserId);

            //builder.HasMany(x => x.Categories).WithOne(x => x.ModifierUser).HasForeignKey(x => x.ModifierUserId);
            //builder.HasMany(x => x.Categories).WithOne(x => x.DeletoryUser).HasForeignKey(x => x.DeletoryUserId);

            //builder.HasMany(x => x.ModifiedComments).WithOne(x => x.ModifierUser).HasForeignKey(x => x.ModifierUserId);
            //builder.HasMany(x => x.DeletedComments).WithOne(x => x.DeletoryUser).HasForeignKey(x => x.DeletoryUserId);

        }
    }
}
