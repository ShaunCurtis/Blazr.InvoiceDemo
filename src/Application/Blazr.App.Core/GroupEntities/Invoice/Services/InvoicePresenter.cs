/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.App.Core;

public class InvoicePresenter
{
    private readonly IDataBroker _dataBroker;
    private ILogger<InvoicePresenter> _logger;

    public InvoicePresenter(IDataBroker dataBroker, ILogger<InvoicePresenter> logger)
    {
        _dataBroker = dataBroker;
        _logger = logger;
    }

    public InvoiceAggregate Item { get; private set; } = new();
    public IDataResult? LastResult { get; private set; }

    public async ValueTask LoadAsync(Guid uid)
    {
        var result = await _dataBroker.GetItemAsync<InvoiceAggregate>(ItemQueryRequest.Create(uid));

        this.LogResult(result);

        if (result.Successful)
            this.Item = result.Item ?? new();
    }

    public async ValueTask SaveAsync()
    {
        var result = await _dataBroker.SaveItemAsync(CommandRequest<InvoiceAggregate>.Create(this.Item));

        this.LogResult(result);

        if (result.Successful)
            this.Item.SetInvoiceAsSaved();
    }

    public async ValueTask DeleteAsync()
    {
        var result = await _dataBroker.DeleteItemAsync(CommandRequest<InvoiceAggregate>.Create(this.Item));

        this.LogResult(result);

        if (result.Successful)
            this.Item.SetInvoiceAsSaved();
    }

    private void LogResult(IDataResult result)
    {
        if (!result.Successful)
            _logger.LogError(result.Message);

        this.LastResult = result;
    }
}
