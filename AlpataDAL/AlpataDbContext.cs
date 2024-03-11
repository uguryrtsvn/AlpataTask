using Microsoft.EntityFrameworkCore;
using AlpataEntities.Entities.Concretes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;
using AlpataEntities.Entities.Base;
using System.Threading;

namespace AlpataDAL
{
    public class AlpataDbContext : DbContext
    {
        public AlpataDbContext(DbContextOptions<AlpataDbContext> options) : base(options)
        {

        }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<MeetingParticipant> MeetingParticipants { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        { 
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetBaseProperty(); 
            return base.SaveChangesAsync(cancellationToken);
        }
        public override int SaveChanges()
        {
            SetBaseProperty();
            return base.SaveChanges();
        }

        private void SetBaseProperty()
        {
            var entries = ChangeTracker.Entries<BaseEntity>();
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added) entry.Entity.CreatedTime = DateTime.Now;
            }
        }
    }
}
