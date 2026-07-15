using GameStore.API.Models;

namespace GameStore.API.Services.Auth;

public interface IJwtTokenService
{
    string GenerateToken(User user);
}
