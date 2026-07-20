using System.ComponentModel.DataAnnotations;

namespace GameStore.API.Dtos.Games;

public class GameFilterDto
{
    public string? SearchTerm { get; set; }

    // Parameter for char tolerance
    [Range(0, 10, ErrorMessage = "Max edit distance must be between 0 and 10")]
    public int MaxEditDistance { get; set; } = 2;

    [Range(0, double.MaxValue, ErrorMessage = "Minimum price must be greater than or equal to 0")]
    public decimal MinPrice { get; set; } = 0;

    [Range(0, double.MaxValue, ErrorMessage = "Maximum price must be greater than or equal to 0")]
    public decimal MaxPrice { get; set; } = decimal.MaxValue;

    public DateOnly StartDate { get; set; } = DateOnly.MinValue;

    public DateOnly EndDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    [Range(0, int.MaxValue, ErrorMessage = "Page number must be at least 1")]
    public int PageNumber { get; set; } = 1;

 //   [Range(1, 1000, ErrorMessage = "Page size must be between 1 and 100")]
    public int PageSize { get; set; }
}
