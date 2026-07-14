using System.ComponentModel.DataAnnotations;

namespace GameStore.API.Dtos.Genres;

public record class UpdateGenreDto
(
    [Required] [StringLength(50)] string Name
);
