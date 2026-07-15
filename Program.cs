using GameStore.API.Configuration;
using GameStore.API.Data;
using GameStore.API.Middlewares;
using GameStore.API.Repositories.Auth;
using GameStore.API.Repositories.Games;
using GameStore.API.Repositories.Genres;
using GameStore.API.Services.Auth;
using GameStore.API.Services.Games;
using GameStore.API.Services.Genres;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configure JWT Settings
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

builder.Services.AddControllers();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Register repositories
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();

// Register services
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

// Configure Database
builder.Services.AddDbContext<GameStoreContext>(options => 
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("neonconnection"));
});

// Configure JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings?.Issuer,
        ValidAudience = jwtSettings?.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings?.Secret ?? ""))
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

app.MapControllers();
app.UseExceptionHandler();

// Add Authentication & Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Migrate DB
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();
    dbContext.Database.Migrate();
}

app.Run();
