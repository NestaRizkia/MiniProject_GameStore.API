using GameStore.API.Data;
using GameStore.API.Middlewares;
using GameStore.API.Repositories.Games;
using GameStore.API.Repositories.Genres;
using GameStore.API.Services.Games;
using GameStore.API.Services.Genres;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Configure Keycloak Authentication & Authorization
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

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

// Register repositories
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();

// Register services
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IGenreService, GenreService>();

// Configure Database
builder.Services.AddDbContext<GameStoreContext>(options => 
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("neonconnection"));
});

var app = builder.Build();

app.UseExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => Results.Ok(new { status = "healthy", message = "GameStore.API is running" }));
app.Run();
