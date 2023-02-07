/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.Infrastructure;

public sealed class ItemRequestAPIHandler
    : IItemRequestHandler
{
    private IHttpClientFactory _factory;

    public ItemRequestAPIHandler(IHttpClientFactory factory)
        => _factory = factory;

    public async ValueTask<ItemQueryResult<TRecord>> ExecuteAsync<TRecord>(ItemQueryRequest request)
        where TRecord : class, new()
    {
        ItemQueryResult<TRecord>? result = null;

        var entityname = (new TRecord()).GetType().Name;

        var httpClient = _factory.CreateClient();
        //TODO - security
        var response = await httpClient.PostAsJsonAsync<ItemQueryRequest>($"/api/{entityname}/listquery", request, request.Cancellation);

        if (response.IsSuccessStatusCode)
            result = await response.Content.ReadFromJsonAsync<ItemQueryResult<TRecord>>();

        return result ?? ItemQueryResult<TRecord>.Failure($"{response.StatusCode} = {response.ReasonPhrase}"); ;
    }
}
