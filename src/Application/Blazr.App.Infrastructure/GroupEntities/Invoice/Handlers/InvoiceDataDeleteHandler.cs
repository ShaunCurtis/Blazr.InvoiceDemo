﻿/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.App.Infrastructure;

public sealed class InvoiceDataDeleteHandler : IDeleteRequestHandler<InvoiceAggregate>
{
    private readonly IDataBroker _broker;
    private ILogger<InvoiceDataItemHandler> _logger;

    public InvoiceDataDeleteHandler(IDataBroker dataBroker, ILogger<InvoiceDataItemHandler> logger)
    {
        _broker = dataBroker;
        _logger = logger;
    }

    public async ValueTask<CommandResult> ExecuteAsync(CommandRequest<InvoiceAggregate> request)
    {
        bool errorTrip = false;

        foreach (var item in request.Item.InvoiceItems)
        {
            var result = await _broker.DeleteItemAsync<DboInvoiceItem>(CommandRequest<DboInvoiceItem>.Create(item.ToDboInvoiceItem()));
            if (!result.Successful)
                _logger.LogError($"InvoiceItem - {item.Uid} - {result.Message}");

            errorTrip = !result.Successful | errorTrip;
        }

        {
            var result = await _broker.DeleteItemAsync<DboInvoice>(CommandRequest<DboInvoice>.Create(request.Item.Invoice.ToDboInvoice()));
            if (!result.Successful)
                _logger.LogError($"InvoiceItem - {request.Item.Invoice.Uid} - {result.Message}");

            errorTrip = !result.Successful | errorTrip;
        }

        return errorTrip
            ? CommandResult.Failure("Failed to delete the invoice")
            : CommandResult.Success();
    }
}
