using AuthECAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services
    .AddIdentityApiEndpoints<AppUser>()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DevDB")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseAuthorization();

app.MapControllers();

app
    .MapGroup("/api")
    .MapIdentityApi<AppUser>();

app.MapPost("/api/signup", async (
    UserManager<AppUser> userManager, 
    [FromBody] UserDTO user
    ) => 
    {
        AppUser appUser = new AppUser
        {
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

app.Run();

public class UserDTO
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }
}