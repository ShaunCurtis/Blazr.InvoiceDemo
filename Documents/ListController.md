## The List Context

We can display a list of Weather Forecasts, but without the context information associated with that list, we don't actually know what it's a list of, and we can't recreate it?  It's therefore important that when we create a list we associate the contexct with the list.

To elaborate, here's my `ListState` object:

```csharp
public class ListState<TRecord>
    where TRecord : class, new()
{
    public string? SortField { get; private set; }
    public bool SortDescending { get; private set; }

    public int PageSize { get; private set; } = 1000;
    public int StartIndex { get; private set; } = 0;
    public int ListTotalCount { get; private set; } = 0;
    public int Page => StartIndex / PageSize;

    public Expression<Func<TRecord, bool>>? FilterExpression { get; private set; }
    public Expression<Func<TRecord, object>>? SortExpression { get; private set; }

    // Lots of setter and getter methods
}
```

It describes the following characteristics of the data set:

1. The filters applied.
2. The order.
3. The data page - the start index and the page size.

The order of the three characteristics is important.  Filter first, then order and finally take the page.

We now define a `RecordList` that links a `List<T>` to it's `ListState`.  This is a materialized list.

1. If the data pipeline returns a `List<>` it holds a reference to that list (such as a Http API pipeline). 
2. If the data pipeline returns an `IEnumerable`or `IQueryable` it materializes it (such as a server side pipeline into a database). 

```csharp
public class RecordList<TRecord> : IEnumerable<TRecord>
    where TRecord : class, new()
{
    private List<TRecord>? _records = new List<TRecord>();
    private ListState<TRecord> _listState = new ListState<TRecord>();

    public ListState<TRecord> ListState => _listState;

    public bool IsPaging => (_listState.PageSize > 0);

    public bool HasList => _records is not null;

    public void Set(ListQueryRequest<TRecord> request, ListQueryResult<TRecord> result)
    {
        _records = result.Items.ToList();
        _listState.Set(request, result);
    }

    public void Reset()
    {
        _records = null;
        _listState.SetPaging(0);
    }

    public IEnumerator<TRecord> GetEnumerator()
    {
        List<TRecord> list = _records ?? new List<TRecord>();
        foreach (var record in list)
            yield return record;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        List<TRecord> list = _records ?? new List<TRecord>();
        foreach (var record in list)
            yield return record;
    }
}
```

The `ListContext` acts as a admin centre providing events and event invocation along with state


```csharp
public sealed class ListContext<TRecord>
    where TRecord : class, new()
{
    private readonly IListContextEventConsumer? _consumer;
    public readonly Guid Id = Guid.NewGuid();

    public event EventHandler<SortRequest>? SortingRequested;
    public event EventHandler<PagingRequest?>? PagingRequested;
    public event EventHandler<ListState<TRecord>>? StateChanged;
    public event EventHandler<EventArgs>? ListChanged;
    public event EventHandler<EventArgs>? PagingReset;

    public ListState<TRecord> ListState { get; private set; } = new ListState<TRecord>();

    public ListContext() => _consumer = null;

    public ListContext(IListContextEventConsumer consumer) => _consumer = consumer;

    public async ValueTask NotifyPagingRequestedAsync(object? sender, PagingRequest? request);
    public async ValueTask NotifySortingRequestedAsync(object? sender, SortRequest request);
    public void NotifyPagingReset(object? sender);
    public void NotifyStateChanged(object? sender, ListState<TRecord> listState);
    public void NotifyListChanged(object? sender);
}
```



