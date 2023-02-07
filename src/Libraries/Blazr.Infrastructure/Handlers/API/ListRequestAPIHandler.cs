/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.Infrastructure;

public sealed class ListRequestAPIHandler
    : IListRequestHandler
{
    private IHttpClientFactory _factory;

    public ListRequestAPIHandler(IHttpClientFactory factory)
        => _factory = factory;

    public async ValueTask<ListQueryResult<TRecord>> ExecuteAsync<TRecord>(ListQueryRequest request)
        where TRecord : class, new()
    {
        ListQueryResult<TRecord>? result = null;

        var entityname = (new TRecord()).GetType().Name;

        var httpClient = _factory.CreateClient();
        //TODO - security
        var response = await httpClient.PostAsJsonAsync<ListQueryRequest>($"/api/{entityname}/listquery", request, request.Cancellation);

        if (response.IsSuccessStatusCode)
            result = await response.Content.ReadFromJsonAsync<ListQueryResult<TRecord>>();

        return result ?? ListQueryResult<TRecord>.Failure($"{response.StatusCode} = {response.ReasonPhrase}"); ;
    }
}
