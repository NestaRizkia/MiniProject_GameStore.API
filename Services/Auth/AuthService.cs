using GameStore.API.Dtos.Auth;
using GameStore.API.Models;
using GameStore.API.Repositories.Auth;
using BCrypt.Net;

namespace GameStore.API.Services.Auth;

public class AuthService(IAuthRepository authRepository, IJwtTokenService jwtTokenService) : IAuthService
{
    public async Task<AuthResponseDto?> RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken)
    {
        // Check if user already exists
        if (await authRepository.UserExistsAsync(registerDto.Username, registerDto.Email, cancellationToken))
        {
            return null;
        }

        // Hash password
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

        // Create user
        var user = new User
        {
            Username = registerDto.Username,
            Email = registerDto.Email,
            PasswordHash = passwordHash,
            Role = "User"
        };

        var createdUser = await authRepository.CreateUserAsync(user, cancellationToken);

        // Generate token
        var token = jwtTokenService.GenerateToken(createdUser);
        var expiresAt = DateTime.UtcNow.AddMinutes(60);

        return new AuthResponseDto
        {
            Token = token,
            Username = createdUser.Username,
            Email = createdUser.Email,
            Role = createdUser.Role,
            ExpiresAt = expiresAt
        };
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken)
    {
        // Find user
        var user = await authRepository.GetUserByUsernameAsync(loginDto.Username, cancellationToken);
        if (user == null)
        {
            return null;
        }

        // Verify password
        if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
        {
            return null;
        }

        // Generate token
        var token = jwtTokenService.GenerateToken(user);
        var expiresAt = DateTime.UtcNow.AddMinutes(60);

        return new AuthResponseDto
        {
            Token = token,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role,
            ExpiresAt = expiresAt
        };
    }
}
