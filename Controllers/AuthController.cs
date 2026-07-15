using GameStore.API.Dtos.Auth;
using GameStore.API.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto registerDto)
    {
        var result = await authService.RegisterAsync(registerDto);
        
        if (result == null)
        {
            return BadRequest(new { message = "Username or email already exists" });
        }

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
    {
        var result = await authService.LoginAsync(loginDto);
        
        if (result == null)
        {
            return Unauthorized(new { message = "Invalid username or password" });
        }

        return Ok(result);
    }
}
