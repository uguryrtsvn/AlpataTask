
using AlpataBLL.Services.Abstracts;
using AlpataBLL.Services.Concretes;
using AlpataBLL.Services.EmailService;
using AlpataBLL.Utilities.IOC;
using AlpataBLL.Utilities.Security.Jwt;
using AlpataDAL.IRepositories;
using AlpataDAL.Repositories;
using AlpataDAL.SeedData;
using Microsoft.Extensions.DependencyInjection;

namespace AlpataBLL.DependencyResolvers
{
    public class BusinessModule : ICoreModule
    {
        public void Load(IServiceCollection services)
        {
            services.AddTransient<ITokenHandler, TokenHandler>();
            services.AddTransient<IAppUserRepository, AppUserRepository>();
            services.AddTransient<IInventoryRepository, InventoryRepository>();
            services.AddTransient<IMeetingRepository, MeetingRepository>();
            services.AddTransient<IMeetingParticipantRepository, MeetingParticipantRepository>();


            services.AddTransient<IMeetingService, MeetingService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IInventoryService, InventoryService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IAuthenticationService, AuthenticationService>();

            services.AddTransient<DbInitializer>();


        }
    }
}
