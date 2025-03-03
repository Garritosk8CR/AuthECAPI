using AuthECAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthECAPI.Controllers
{
    public static class IdentityUserEndpoints
    {
        public static IEndpointRouteBuilder MapIdentityUserEndpoints(this IEndpointRouteBuilder app)
        {
            
            app.MapPost("/signup", CreateUser);
            app.MapPost("/signin",SignIn);
            return app;
        }
        [AllowAnonymous]
        public static async Task<IResult> CreateUser(UserManager<AppUser> userManager, [FromBody] UserRegistrationDTO user) {
            AppUser appUser = new AppUser
            {
                UserName = user.Email,
                Email = user.Email,
                FullName = user.FullName
            };

            var result = await userManager.CreateAsync(appUser, user.Password);
            if (result.Succeeded)
            {
                return Results.Created($"/api/users/{appUser.Id}", appUser);
            }
            else
            {
                return Results.BadRequest(result.Errors);
            }
        }
        [AllowAnonymous]
        public static async Task<IResult> SignIn(UserManager<AppUser> userManager, [FromBody] UserLoginDTO user, IOptions<AppSettings>appSettings)
        {
            var userfound = await userManager.FindByEmailAsync(user.Email);
            if (userfound != null && await userManager.CheckPasswordAsync(userfound, user.Password))
            {
                var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(appSettings.Value.JWTSecret)
                );

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                            new Claim("UserID", userfound.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(10),
                    SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);

                var token = tokenHandler.WriteToken(securityToken);

                return Results.Ok(new { token });
            }
            else
                return Results.BadRequest(new { message = "password or email is incorrect" });
        }
    }

    public class UserRegistrationDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
    }

    public class UserLoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
