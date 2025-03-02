

using AuthECAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthECAPI.Extensions
{
    public static class AppConfigExtensions
    {
        public static WebApplication ConfigCORS(this WebApplication app, IConfiguration configuration)
        {
            app.UseCors(
                options => options
                    .WithOrigins("http://localhost:4200")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
            );
            return app;
        }

        public static IServiceCollection AddAppConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppSettings>(configuration.GetSection("AppSettings"));
            return services;
        }
    }
}
