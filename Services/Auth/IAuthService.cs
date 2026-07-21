using GameStore.API.Dtos.Auth;

namespace GameStore.API.Services.Auth;

public interface IAuthService
{
    Task<AuthResponseDto?> RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken);
    Task<AuthResponseDto?> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken);
}
