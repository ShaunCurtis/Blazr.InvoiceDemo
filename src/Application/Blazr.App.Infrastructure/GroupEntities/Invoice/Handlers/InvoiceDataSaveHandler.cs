/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.App.Infrastructure;

public sealed class InvoiceDataSaveHandler : ISaveRequestHandler<InvoiceData>
{
    private readonly IDataBroker _broker;
    private ILogger<InvoiceDataItemHandler> _logger;

    public InvoiceDataSaveHandler(IDataBroker dataBroker, ILogger<InvoiceDataItemHandler> logger)
    {
        _broker = dataBroker;
        _logger = logger;
    }

    public async ValueTask<CommandResult> ExecuteAsync(CommandRequest<InvoiceData> request)
    {
        bool errorTrip = false;

        var invoiceData = request.Item;
        // Create the invoice if it's new
        if (invoiceData.IsNewInvoice)
        {
            var result = await _broker.CreateItemAsync<Invoice>(CommandRequest<Invoice>.Create(invoiceData.Invoice.ToInvoice()));
            if (!result.Successful)
                _logger.LogError($"Invoice - {invoiceData.Invoice.Uid} - {result.Message}");

            errorTrip = !result.Successful | errorTrip;
        }

        // Update the invoice if it is dirty)
        if (!invoiceData.IsNewInvoice && invoiceData.InvoiceIsDirty)
        {
            var result = await _broker.UpdateItemAsync<Invoice>(CommandRequest<Invoice>.Create(invoiceData.Invoice.ToInvoice()));
            if (!result.Successful)
                _logger.LogError($"Invoice - {invoiceData.Invoice.Uid} - {result.Message}");

            errorTrip = !result.Successful | errorTrip;
        }

        // Update all the existing items
        foreach (var item in invoiceData.UpdatedItems)
        {
            var result = await _broker.UpdateItemAsync<InvoiceItem>(CommandRequest<InvoiceItem>.Create(item.ToInvoiceItem()));
            if (!result.Successful)
                _logger.LogError($"Invoice Item - {item.Uid} - {result.Message}");

            errorTrip = !result.Successful | errorTrip;
        }

        // Add all the new items
        foreach (var item in invoiceData.AddedItems)
        {
            var result = await _broker.CreateItemAsync<InvoiceItem>(CommandRequest<InvoiceItem>.Create(item.ToInvoiceItem()));
            if (!result.Successful)
                _logger.LogError($"Invoice Item - {item.Uid} - {result.Message}");

            errorTrip = !result.Successful | errorTrip;
        }

        // Remove all the items in the deleted collection
        foreach (var item in invoiceData.DeletedItems)
        {
            var result = await _broker.DeleteItemAsync<InvoiceItem>(CommandRequest<InvoiceItem>.Create(item.ToInvoiceItem()));
            if (!result.Successful)
                _logger.LogError($"Invoice Item - {item.Uid} - {result.Message}");

            errorTrip = !result.Successful | errorTrip;
        }

        return errorTrip
            ? CommandResult.Failure("Failed to update the invoice")
            : CommandResult.Success();
    }
}
