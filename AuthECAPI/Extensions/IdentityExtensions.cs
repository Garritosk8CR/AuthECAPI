using AuthECAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthECAPI.Extensions
{
    public static class IdentityExtensions
    {
        public static void AddIdentityHandlers(this IServiceCollection services)
        {
            services
                .AddIdentityApiEndpoints<AppUser>()
                .AddEntityFrameworkStores<AppDbContext>();
        }

        public static void ConfigureIdentityOptions(this IServiceCollection services)
        {
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.User.RequireUniqueEmail = true;
            });
        }
    }
}
