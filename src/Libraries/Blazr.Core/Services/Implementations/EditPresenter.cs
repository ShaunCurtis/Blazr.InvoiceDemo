/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.Core;

public sealed class EditPresenter<TRecord, TEntityService, TEditContext> : IEditPresenter<TRecord, TEditContext>
    where TRecord : class, new()
    where TEditContext : class, IEditContext, IRecordEditContext<TRecord>, new()
    where TEntityService : class, IEntityService
{
    private readonly IDataBroker _dataBroker;
    private readonly INotificationService<TEntityService> _notificationService;

    public CommandResult LastCommand { get; private set; } = CommandResult.Success();

    public ItemQueryResult<TRecord> LastResult { get; private set; } = ItemQueryResult<TRecord>.Success(new());

    public TEditContext EditStateContext { get; private set; } = new();

    public EditPresenter(IDataBroker dataBroker, INotificationService<TEntityService> notificationService)
    { 
        _dataBroker = dataBroker;
        _notificationService = notificationService;
    }

    public async ValueTask LoadAsync(Guid id)
        => await GetItemAsync(new ItemQueryRequest { Uid = id });

    public ValueTask ResetItemAsync()
    {
        EditStateContext.Reset();
        return ValueTask.CompletedTask;
    }

    public ValueTask SaveItemAsync()
        => this.SaveAsync();

    private async ValueTask GetItemAsync(ItemQueryRequest request)
    {
        ItemQueryResult<TRecord> result = await _dataBroker.GetItemAsync<TRecord>(request);

        if (result.Successful && result.Item is not null)
            EditStateContext.Load(result.Item, true);
    }

    private async ValueTask SaveAsync()
    {
        LastCommand = CommandResult.Failure("Nothing to Save");

        if (!this.EditStateContext.IsDirty)
            return;

        var request = new CommandRequest<TRecord> { Item = this.EditStateContext.Record };

        if (this.EditStateContext.IsNew)
            LastCommand = await _dataBroker.CreateItemAsync<TRecord>(request);
        else
            LastCommand = await _dataBroker.UpdateItemAsync<TRecord>(request);

        if (LastCommand.Successful)
            EditStateContext.SetAsSaved();

        _notificationService.NotifyRecordChanged(this, this.EditStateContext.Record);
    }
}