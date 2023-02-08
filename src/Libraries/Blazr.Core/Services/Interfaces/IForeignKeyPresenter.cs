/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.Core;

public interface IForeignKeyPresenter<TFkItem, TEntityService>
    where TFkItem : class, IFkItem, new()
    where TEntityService : class, IEntityService
{
    public IEnumerable<IFkItem> Items { get; }

    public ValueTask<bool> GetFkList();
}
