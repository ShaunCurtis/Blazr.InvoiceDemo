namespace Blazr.App.Infrastructure;

public record CountryData
{
    public required string Country { get; init; }
    public required string Continent { get; init; }
}
