using AuthECAPI.Extensions;
using AuthECAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.InjectDbContext(builder.Configuration)
                .AddIdentityHandlers()
                .ConfigureIdentityOptions()            
                .AddIdentityAuth(builder.Configuration)
                .AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.AddScalarExplorer()
    .ConfigCORS(builder.Configuration);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app
    .MapGroup("/api")
    .MapIdentityApi<AppUser>();

app.MapPost("/api/signup", async (UserManager<AppUser> userManager, [FromBody] UserRegistrationDTO user) => 
    {
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
);

app.MapPost("/api/signin", async (UserManager<AppUser> userManager, [FromBody] UserLoginDTO user) =>
    {
        var userfound = await userManager.FindByEmailAsync(user.Email);
        if (userfound != null && await userManager.CheckPasswordAsync(userfound, user.Password))
        {
            var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["appSettings:JWTSecret"]!)
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
            return Results.BadRequest(new { message = "password or email is incorrect"});
    }
);

app.Run();

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