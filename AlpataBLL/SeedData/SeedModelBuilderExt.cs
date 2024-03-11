  
using AlpataEntities.Entities.Concretes;
using AlpataBLL.Utilities.Hashing;

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
            HashingHelper.CreatePassword("1234", out byte[] passwordHash, out byte[] passwordSalt);
            AppUser user = new AppUser
            {
                Id = Guid.NewGuid(),
                Name = "alpata",
                Surname = "alpata",
                UserName = "alpata@alpata.com",
                NormalizedUserName = "ALPATA@ALPATA.COM",
                Email = "alpata@alpata.com",
                NormalizedEmail = "ALPATA@ALPATA.COM",
                EmailConfirmed = true, 
                CreatedTime = DateTime.Now,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                ImagePath = "sdfsdf",
                Phone = "12312312312"
            }; 
            context.Set<AppUser>().Add(user);
            context.SaveChanges(); 
        }


    }
}
