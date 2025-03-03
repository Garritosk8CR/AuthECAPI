namespace AuthECAPI.Controllers
{
    public static class AccountEndpoints
    {
        public static IEndpointRouteBuilder MapAccountEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/userProfile", GetUserProfile);
            return app;
        }

        private static async Task<string> GetUserProfile(HttpContext context)
        {
            return "User profile";
        }
    }
}
