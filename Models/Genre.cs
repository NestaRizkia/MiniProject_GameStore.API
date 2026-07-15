namespace GameStore.API.Models;

public class Genre
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}