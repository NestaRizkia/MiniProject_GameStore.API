using System.ComponentModel.DataAnnotations;

namespace GameStore.API.Dtos.Auth;

public class LoginDto
{
    [Required(ErrorMessage = "Username is required")]
    public required string Username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public required string Password { get; set; }
}
