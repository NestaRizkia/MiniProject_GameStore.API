using GameStore.API.Configuration;
using GameStore.API.Data;
using GameStore.API.Middlewares;
using GameStore.API.Modules.Games;
using GameStore.API.Modules.Genres;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Keycloak:Authority"];
        options.Audience = builder.Configuration["Keycloak:Audience"];
        options.RequireHttpsMetadata = builder.Configuration.GetValue<bool>("Keycloak:RequireHttpsMetadata", false);
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Keycloak:Authority"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Keycloak:Audience"],
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
    });

builder.Services.Configure<UMAOptions>(builder.Configuration.GetSection("UMA"));

// Register modules
builder.Services.AddGamesModule();
builder.Services.AddGenresModule();

builder.Services.AddDbContext<GameStoreContext>(options => 
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("neonconnection"));
});

var app = builder.Build();

app.UseExceptionHandler();

app.UseAuthentication();
app.UseMiddleware<KeycloakAuthorizationMiddleware>();
app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => Results.Ok(new { status = "healthy", message = "GameStore.API is running" }));
app.Run();
