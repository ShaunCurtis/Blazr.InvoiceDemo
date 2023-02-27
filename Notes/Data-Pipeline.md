#  Data Pipeline

The application data pipeline is a composite design incorporating themes from several coding patterns.  It's based on a melding of the Repository and CQS patterns.

The "contract" between the the Core Domain and the Infrastructure domain is defined in a Data Broker interface.

```csharp
public interface IDataBroker
{
    public ValueTask<ListQueryResult<TRecord>> GetItemsAsync<TRecord>(ListQueryRequest request) where TRecord : class, new();

    public ValueTask<ItemQueryResult<TRecord>> GetItemAsync<TRecord>(ItemQueryRequest request) where TRecord : class, new();

    public ValueTask<CommandResult> UpdateItemAsync<TRecord>(CommandRequest<TRecord> request) where TRecord : class, new();

    public ValueTask<CommandResult> CreateItemAsync<TRecord>(CommandRequest<TRecord> request) where TRecord : class, new();

    public ValueTask<CommandResult> DeleteItemAsync<TRecord>(CommandRequest<TRecord> request) where TRecord : class, new();

    public ValueTask<CommandResult> SaveItemAsync<TRecord>(CommandRequest<TRecord> request) where TRecord : class, new();
}
```

There are some key design points to take from this definition:

1. Generics are applied at the method level, not the class level.  There's one concrete implementation that impelements the interface for all data classes.
2. Each method is passed a "Request" object and returns a "Result" object.
3. Everything is Task based and returns a `ValueTask`.

The implementation class looks like this.  I've shortened it to concentrate on just the record getter.  Note that we inject `IItemRequestHandler` and then call the `ExecuteAsync` method on the interface.  Each operation in the Data Broker has a corresponding Handler interface.


```csharp
public sealed class RepositoryDataBroker : IDataBroker
{
    private readonly IItemRequestHandler _itemRequestHandler;
    //...

    public RepositoryDataBroker(
        IItemRequestHandler itemRequestHandler,
        //...
        )
    {
        _itemRequestHandler = itemRequestHandler;
        //...
    }

    public ValueTask<ItemQueryResult<TRecord>> GetItemAsync<TRecord>(ItemQueryRequest request) where TRecord : class, new()
        => _itemRequestHandler.ExecuteAsync<TRecord>(request);

    //...
}
```

### IItemRequestHandler

IItemRequestHandler has a typed and non-typed version.

```csharp
public interface IItemRequestHandler
{
    public ValueTask<ItemQueryResult<TRecord>> ExecuteAsync<TRecord>(ItemQueryRequest request)
        where TRecord : class, new();
}

public interface IItemRequestHandler<TRecord>
        where TRecord : class, new()
{
    public ValueTask<ItemQueryResult<TRecord>> ExecuteAsync(ItemQueryRequest request);
}
```

### ItemRequestServerHandler

The server Handler looks like this.  It injects the base Handler which we'll see shortly and the `IServiceProvider`.

The internal `_getItemsAsync`:

1.  Attempts to get a customer `IListRequestHandler` registered against the specific record `TRecord`.
2.  If it finds one it executes the custom handler and returns the result.
3.  If it doesn't it executes the standard handler and returns the result.

The handler acts as a mediator and can be coded to do whatever actions you want on a record.  A custom handler can do validation checks through Fluent Validation, apply guards or object translations.

```csharp
public sealed class ListRequestServerHandler<TDbContext> 
    : IListRequestHandler
    where TDbContext : DbContext
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ListRequestBaseServerHandler<TDbContext> _baseHandler;

    public ListRequestServerHandler(IServiceProvider serviceProvider, ListRequestBaseServerHandler<TDbContext> baseHandler)
    {
        _serviceProvider = serviceProvider;
        _baseHandler = baseHandler;
    }

    public ValueTask<ListQueryResult<TRecord>> ExecuteAsync<TRecord>(ListQueryRequest request)
        where TRecord : class, new()
            => _getItemsAsync<TRecord>(request);

    private async ValueTask<ListQueryResult<TRecord>> _getItemsAsync<TRecord>(ListQueryRequest request)
        where TRecord : class, new()
    {
        IListRequestHandler<TRecord>? _customHandler = null;

        // Try and get a registered custom handler
        try
        {
            _customHandler = _serviceProvider.GetComponentService<IListRequestHandler<TRecord>>();
        }
        catch { }   

        // If we get one then one is registered in DI and we execute it
        if (_customHandler is not null)
            return await _customHandler.ExecuteAsync(request);

        // If there's no custom handler registered we run the base handler
        return await _baseHandler.ExecuteAsync<TRecord>(request);
    }
}
```
### ItemRequestBaseServerHandler

The base handler does the actual work.  Making the call into EF, getting the record and handling any errors.

```csharp
public sealed class ItemRequestBaseServerHandler<TDbContext>
    : IItemRequestHandler
    where TDbContext : DbContext
{
    private readonly IDbContextFactory<TDbContext> _factory;

    public ItemRequestBaseServerHandler(IDbContextFactory<TDbContext> factory)
        => _factory = factory;

    public async ValueTask<ItemQueryResult<TRecord>> ExecuteAsync<TRecord>(ItemQueryRequest request)
        where TRecord : class, new()
    {
        if (request == null)
            throw new DataPipelineException($"No ListQueryRequest defined in {GetType().FullName}");

        using var dbContext = _factory.CreateDbContext();
        dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

        TRecord? record = null;

        // first check if the record implements IGuidIdentity.  If so we can do a cast and then do the query via the Uid property directly 
        if (new TRecord() is IGuidIdentity)
            record = await dbContext.Set<TRecord>().SingleOrDefaultAsync(item => ((IGuidIdentity)item).Uid == request.Uid, request.Cancellation);

        // Try and use the EF FindAsync implementation
        if (record is null)
            record = await dbContext.FindAsync<TRecord>(request.Uid);

        if (record is null)
            return ItemQueryResult<TRecord>.Failure("No record retrieved");

        return ItemQueryResult<TRecord>.Success(record);
    }
}
```

To demonstrate, here's a custom `IItemRequestHandler` for the `Customer` data record that just makes a call into the base to get the record.


```csharp
public sealed class CustomerRequestServerHandler<TDbContext>
    : IItemRequestHandler<Customer>
    where TDbContext : DbContext
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ItemRequestBaseServerHandler<TDbContext> _baseHandler;

    public CustomerRequestServerHandler(IServiceProvider serviceProvider, ItemRequestBaseServerHandler<TDbContext> serverHandler)
    {
        _serviceProvider = serviceProvider;
        _baseHandler = serverHandler;
    }

    public async ValueTask<ItemQueryResult<Customer>> ExecuteAsync(ItemQueryRequest request)
    {
        return await _baseHandler.ExecuteAsync<Customer>(request);
    }
}

```
