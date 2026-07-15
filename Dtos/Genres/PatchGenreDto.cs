using System.ComponentModel.DataAnnotations;

namespace GameStore.API.Dtos.Genres;

public class PatchGenreDto
{
    [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
    public string? Name { get; set; }
}
