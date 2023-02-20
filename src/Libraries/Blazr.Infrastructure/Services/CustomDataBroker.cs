/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.Infrastructure;

public sealed class CustomDataBroker : ICustomDataBroker
{
    private IServiceProvider _serviceProvider;

    public CustomDataBroker(IServiceProvider serviceProvider)
        => _serviceProvider = serviceProvider;

    public async ValueTask<ItemQueryResult<TRecord>> GetItemAsync<TRecord>(ItemQueryRequest request) where TRecord : class, new()
    {
        var handler = _serviceProvider.GetService<IItemRequestHandler<TRecord>>();
        ArgumentNullException.ThrowIfNull(handler);

        return await handler.ExecuteAsync(request);
    }

    public async ValueTask<CommandResult> SaveItemAsync<TRecord>(CommandRequest<TRecord> command) where TRecord : class, new()
    {
        var handler = _serviceProvider.GetService<ISaveRequestHandler<TRecord>>();
        ArgumentNullException.ThrowIfNull(handler);

        return await handler.ExecuteAsync(command);
    }

    public async ValueTask<CommandResult> DeleteItemAsync<TRecord>(CommandRequest<TRecord> command) where TRecord : class, new()
    {
        var handler = _serviceProvider.GetService<IDeleteRequestHandler<TRecord>>();
        ArgumentNullException.ThrowIfNull(handler);

        return await handler.ExecuteAsync(command);
    }
}
