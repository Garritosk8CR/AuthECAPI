using AuthECAPI.Controllers;
using AuthECAPI.Extensions;
using AuthECAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.InjectDbContext(builder.Configuration)
                .AddAppConfig(builder.Configuration)
                .AddIdentityHandlers()
                .ConfigureIdentityOptions()            
                .AddIdentityAuth(builder.Configuration)
                .AddOpenApi();



var app = builder.Build();

// Configure the HTTP request pipeline.
app.AddScalarExplorer()
   .ConfigCORS(builder.Configuration)
   .AddIdentityAuthMiddlewares();

app.MapControllers();

app.MapGroup("/api")
    .MapIdentityApi<AppUser>();

app.MapGroup("/api")
    .MapIdentityUserEndpoints();

app.Run();