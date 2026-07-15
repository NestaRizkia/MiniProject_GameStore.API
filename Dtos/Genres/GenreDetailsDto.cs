namespace GameStore.API.Dtos.Genres;

public record class GenreDetailsDto
(
    int Id,
    string Name,
    DateTime CreatedAt,
    DateTime UpdatedAt
);