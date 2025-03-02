using AuthECAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AuthECAPI.Extensions
{
    public static class IdentityExtensions
    {
        public static IServiceCollection AddIdentityHandlers(this IServiceCollection services)
        {
            services
                .AddIdentityApiEndpoints<AppUser>()
                .AddEntityFrameworkStores<AppDbContext>();
            return services;
        }

        public static IServiceCollection ConfigureIdentityOptions(this IServiceCollection services)
        {
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.User.RequireUniqueEmail = true;
            });
            return services;
        }

        public static IServiceCollection AddIdentityAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme =
                x.DefaultChallengeScheme =
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(y =>
            {
                y.SaveToken = false;
                y.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["appSettings:JWTSecret"]!)
                        ),

                };
            });
            return services;
        }

        public static WebApplication AddIdentityAuthMiddlewares(this WebApplication app)
        {
            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }
    }
}
