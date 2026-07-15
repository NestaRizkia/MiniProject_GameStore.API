namespace GameStore.API.Models;

public class User
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public string Role { get; set; } = "User";
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
