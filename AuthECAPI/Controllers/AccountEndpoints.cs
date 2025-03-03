using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AuthECAPI.Controllers
{
    public static class AccountEndpoints
    {
        public static IEndpointRouteBuilder MapAccountEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/userProfile", GetUserProfile);
            return app;
        }
        [Authorize]
        private static async Task<string> GetUserProfile(ClaimsPrincipal user)
        {
            string userId = user.Claims.First(x => x.Type == "UserID").Value;
            return "User profile";
        }
    }
}
