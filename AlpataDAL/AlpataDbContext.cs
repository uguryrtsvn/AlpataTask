using Microsoft.EntityFrameworkCore;
using AlpataEntities.Entities.Concretes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AlpataDAL
{
    public class AlpataDbContext:DbContext
    {
        public AlpataDbContext(DbContextOptions<AlpataDbContext> options) : base(options)
        {

        }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Meeting> Meetings { get; set; } 
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Mevcut katmandaki bütün IEntityTypeConfiguration interface'inden türeyen configurasyonları uygulaması için

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}
