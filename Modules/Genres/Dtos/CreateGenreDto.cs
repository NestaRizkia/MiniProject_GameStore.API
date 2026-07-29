using System.ComponentModel.DataAnnotations;

namespace GameStore.API.Modules.Genres.Dtos;

public record class CreateGenreDto
(
    [Required] [StringLength(50)] string Name
);
