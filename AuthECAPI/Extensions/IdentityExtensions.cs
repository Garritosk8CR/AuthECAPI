using AuthECAPI.Models;

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
    }
}
