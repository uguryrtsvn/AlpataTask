 
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Reflection.Emit;
using AlpataEntities.Entities.Concretes;

namespace AlpataDAL.SeedData
{
    public static class SeedModelBuilderExt
    {
        public static void Seed(this AlpataDbContext context)
        {  
            if (context.Set<AppUser>().Any())
            {
                // Veritabanında veri varsa seed işlemini atla
                return;
            } 
            context.SaveChanges();
            var hasher = new PasswordHasher<AppUser>(); 
            AppUser user = new AppUser
            {
                Id = Guid.NewGuid(),
                FirstName = "alpata",
                LastName = "alpata",
                UserName = "alpata@alpata.com",
                NormalizedUserName = "ALPATA@ALPATA.COM",
                Email = "alpata@alpata.com",
                NormalizedEmail = "ALPATA@ALPATA.COM",
                EmailConfirmed = true, 
                CreatedTime = DateTime.Now, 
                PasswordHash = hasher.HashPassword(null, "alpata"), 
                LockoutEnabled = false
            };
            context.Set<AppUser>().Add(user);
            context.SaveChanges(); 
        }


    }
}
