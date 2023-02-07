/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Core;

public sealed class WeatherSummaryPresenter
{
    private IDataBroker _dataBroker;

    public IEnumerable<WeatherSummary> Items { get; private set; } = new List<WeatherSummary>();

    public WeatherSummaryPresenter(IDataBroker dataBroker)
        => _dataBroker = dataBroker;

    public async ValueTask LoadAsync()
    {
        var request = new ListQueryRequest<WeatherSummary>();
        ListQueryResult<WeatherSummary> result = await _dataBroker.GetItemsAsync<WeatherSummary>(request);

        if(result.Successful)
            this.Items= result.Items;
    }
}