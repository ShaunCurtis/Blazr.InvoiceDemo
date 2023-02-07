/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.Infrastructure;

public sealed class DeleteRequestAPIHandler
    : IDeleteRequestHandler
{
    private IHttpClientFactory _factory;

    public DeleteRequestAPIHandler(IHttpClientFactory factory)
        => _factory = factory;

    public async ValueTask<CommandResult> ExecuteAsync<TRecord>(CommandRequest<TRecord> request)
        where TRecord : class, new()
    {
        CommandResult? result = null;

        var entityname = (new TRecord()).GetType().Name;

        var httpClient = _factory.CreateClient();
        //TODO - security
        var response = await httpClient.PostAsJsonAsync<CommandRequest<TRecord>>($"/api/{entityname}/deleterecordcommand", request, request.Cancellation);

        if (response.IsSuccessStatusCode)
            result = await response.Content.ReadFromJsonAsync<CommandResult>();

        return result ?? CommandResult.Failure($"{response.StatusCode} = {response.ReasonPhrase}"); ;
    }
}
