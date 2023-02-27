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

