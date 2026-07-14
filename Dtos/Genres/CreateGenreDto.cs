using System.ComponentModel.DataAnnotations;

namespace GameStore.API.Dtos.Genres;

public record class CreateGenreDto
(
    [Required] [StringLength(50)] string Name
);
