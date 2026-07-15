using System.ComponentModel.DataAnnotations;

namespace GameStore.API.Dtos.Games;

public class PatchGameDto
{
    [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
    public string? Name { get; set; }

    [Range(1, 50, ErrorMessage = "GenreId must be between 1 and 50")]
    public int? GenreId { get; set; }

    [Range(1, 100, ErrorMessage = "Price must be between 1 and 100")]
    public decimal? Price { get; set; }

    public DateOnly? ReleaseDate { get; set; }
}
