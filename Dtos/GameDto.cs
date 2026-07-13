namespace GameStore.API.Dtos;

// Dto is a Contract between cliend and the server since it represent
// A shared aggreement on how the data will be transferred and used
public record class GameDto
(
    int Id,
    string Name,
    string Genre,
    decimal Price,
    DateOnly ReleaseData
);