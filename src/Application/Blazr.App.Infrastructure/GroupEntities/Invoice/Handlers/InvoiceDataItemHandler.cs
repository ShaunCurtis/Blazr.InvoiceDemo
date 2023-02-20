/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.App.Infrastructure;

public sealed class InvoiceDataItemHandler : IItemRequestHandler<InvoiceData>
{
    private readonly IDataBroker _broker;
    private ILogger<InvoiceDataItemHandler> _logger;

    public InvoiceDataItemHandler(IDataBroker dataBroker, ILogger<InvoiceDataItemHandler> logger)
    {
        _broker = dataBroker;
        _logger = logger;
    }

    public async ValueTask<ItemQueryResult<InvoiceData>> ExecuteAsync(ItemQueryRequest request)
    {
        var result = await _broker.GetItemAsync<Invoice>(ItemQueryRequest.Create(request.Uid));

        var invoice = result.Item ?? new();

        // Check if we got an invoice or have defaulted to a new invoice
        if (!result.Successful)
            return ItemQueryResult<InvoiceData>.Failure("Could Not find requested Invoice");

        // Get the items assocaited with the invoice
        var filters = new List<FilterDefinition> { new FilterDefinition { FilterName = ApplicationConstants.ByInvoiceUid, FilterData = request.Uid.ToString(), } };
        var query = new ListQueryRequest() { Filters = filters };
        var listResult = await _broker.GetItemsAsync<InvoiceItem>(query);

        if (listResult.Successful)
            return ItemQueryResult<InvoiceData>.Success(new(invoice, listResult.Items));

        return ItemQueryResult<InvoiceData>.Failure("Could Not fufill the request.");
    }
}
