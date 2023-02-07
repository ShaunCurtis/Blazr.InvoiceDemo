/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Blazr.App.Infrastructure;

public sealed class CountryDataProvider
{
    private readonly List<CountryData> _baseDataSet;
    public Task LoadTask { get; private set; } = Task.CompletedTask;

    private List<Continent> _continents = new();
    private List<Country> _countries = new();

    public CountryDataProvider(IOptions<List<CountryData>> options)
    {
        _baseDataSet = options.Value;
        this.LoadTask = LoadBaseData();
    }

    public async ValueTask<IEnumerable<Country>> GetCountriesAsync()
    {
        await this.LoadTask;
        return _countries.AsEnumerable();
    }

    public async ValueTask<IEnumerable<Continent>> GetContinentsAsync()
    {
        await this.LoadTask;
        return _continents.AsEnumerable();
    }

    public async ValueTask<IEnumerable<Country>> FilteredCountries(string? searchText, Guid? continentUid = null)
        => await this.GetFilteredCountries(searchText, continentUid);

    public async ValueTask<IEnumerable<Country>> FilteredCountriesAsync(Guid continentUid)
    {
        await this.LoadTask;
        return _countries.Where(item => item.ContinentUid == continentUid);
    }

    private Task LoadBaseData()
    {
        var distinctContinentNames = _baseDataSet.Select(item => item.Continent).Distinct().ToList();

        foreach (var continent in distinctContinentNames)
            _continents.Add(new Continent { Name = continent });

        foreach (var continent in _continents)
        {
            var countryNamesInContinent = _baseDataSet.Where(item => item.Continent == continent.Name).Select(item => item.Country).ToList();

            foreach (var countryName in countryNamesInContinent)
                _countries.Add(new Country { Name = countryName, ContinentUid = continent.Uid });
        }
        return Task.CompletedTask;
    }

    private async ValueTask<IEnumerable<Country>> GetFilteredCountries(string? searchText, Guid? continentUid = null)
    {
        await this.LoadTask;

        var query = _countries.AsEnumerable();

        if (continentUid is not null && continentUid != Guid.Empty)
            query = query.Where(item => item.ContinentUid == continentUid);

        if (!string.IsNullOrWhiteSpace(searchText))
            query = query.Where(item => item.Name.ToLower().Contains(searchText.ToLower()));

        return query.OrderBy(item => item.Name);
    }
}
