using AuthECAPI.Models;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

namespace AuthECAPI.Extensions
{
    public static class ScalarExtensions
    {
        public static WebApplication AddScalarExplorer(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }
            return app;
        }
    }
}
