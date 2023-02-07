/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.Core;

public interface IListPresenter<TRecord, TEntityService> 
    where TRecord : class, new()
    where TEntityService : class, IEntityService
{
    public ListController<TRecord> ListController { get; }

    public int DefaultPageSize { get; set; }

    public Guid StateId { get; set; }

    public ValueTask GetItemsAsync(ListQueryRequest request, object? sender = null);

    public ValueTask<ItemsProviderResult<TRecord>> GetItemsAsync(ItemsProviderRequest itemsRequest);
}
