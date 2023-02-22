/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.App.Infrastructure;

public sealed class InvoiceDataDeleteHandler : IDeleteRequestHandler<InvoiceData>
{
    private readonly IDataBroker _broker;
    private ILogger<InvoiceDataItemHandler> _logger;

    public InvoiceDataDeleteHandler(IDataBroker dataBroker, ILogger<InvoiceDataItemHandler> logger)
    {
        _broker = dataBroker;
        _logger = logger;
    }

    public async ValueTask<CommandResult> ExecuteAsync(CommandRequest<InvoiceData> request)
    {
        bool errorTrip = false;

        foreach (var item in request.Item.InvoiceItems)
        {
            var result = await _broker.DeleteItemAsync<InvoiceItem>(CommandRequest<InvoiceItem>.Create(item.ToInvoiceItem()));
            if (!result.Successful)
                _logger.LogError($"InvoiceItem - {item.Uid} - {result.Message}");

            errorTrip = !result.Successful | errorTrip;
        }

        {
            var result = await _broker.DeleteItemAsync<Invoice>(CommandRequest<Invoice>.Create(request.Item.Invoice.ToInvoice()));
            if (!result.Successful)
                _logger.LogError($"InvoiceItem - {request.Item.Invoice.Uid} - {result.Message}");

            errorTrip = !result.Successful | errorTrip;
        }

        return errorTrip
            ? CommandResult.Failure("Failed to delete the invoice")
            : CommandResult.Success();
    }
}
