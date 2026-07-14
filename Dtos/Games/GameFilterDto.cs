using System.ComponentModel.DataAnnotations;

namespace GameStore.API.Dtos.Games;

public class GameFilterDto
{
    // Search filter - case-insensitive search on Name and Genre
    public string? SearchTerm { get; set; }

    // Price range filters
    [Range(0, double.MaxValue, ErrorMessage = "Minimum price must be greater than or equal to 0")]
    public decimal? MinPrice { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Maximum price must be greater than or equal to 0")]
    public decimal? MaxPrice { get; set; }

    // Date range filters
    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    // Pagination parameters
    [Range(1, int.MaxValue, ErrorMessage = "Page number must be at least 1")]
    public int PageNumber { get; set; } = 1;

    [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100")]
    public int PageSize { get; set; } = 10;
}
