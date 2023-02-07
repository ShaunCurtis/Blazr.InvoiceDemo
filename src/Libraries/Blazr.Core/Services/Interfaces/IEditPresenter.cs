/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.Core;

public interface IEditPresenter<TRecord, TEditContext>
    where TRecord : class, new()
    where TEditContext : class, IEditContext, IRecordEditContext<TRecord>, new()
{
    public CommandResult LastCommand { get; }

    public ItemQueryResult<TRecord> LastResult { get; }

    public TEditContext EditStateContext { get; }

    public ValueTask LoadAsync(Guid id);

    public ValueTask ResetItemAsync();

    public  ValueTask SaveItemAsync();
}