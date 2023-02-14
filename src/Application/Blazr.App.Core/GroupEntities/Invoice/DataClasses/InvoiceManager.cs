/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.App.Core;

public sealed class InvoiceManager : IGuidIdentity
{
    public Guid Uid { get; init; } = Guid.NewGuid();

    public Invoice Invoice { get; private set; } = new Invoice();

    public IEnumerable<InvoiceItem> InvoiceItems => _invoiceItems;

    private List<InvoiceItem> _invoiceItems { get; set; } = new();
    private IDataBroker _broker;
    private ILogger<InvoiceManager> _logger;

    public InvoiceManager(IDataBroker dataBroker, ILogger<InvoiceManager> logger)
    {
        _broker = dataBroker;
        _logger = logger;
    }

    public async ValueTask LoadAsync(Guid uid)
    {
        var result = await _broker.GetItemAsync<Invoice>(ItemQueryRequest.Create(uid));
        this.Invoice = result.Item ?? new();

        var filters = new List<FilterDefinition> { new FilterDefinition { FilterName = ApplicationConstants.ByInvoiceUid, FilterData = uid.ToString(), } };
        var query = new ListQueryRequest() { Filters=filters };
        var listResult = await _broker.GetItemsAsync<InvoiceItem>(query);
    }

    public void Load(Invoice invoice, List<InvoiceItem> invoiceItems)
    {
        Invoice = invoice;
        _invoiceItems = invoiceItems;
    }

    public async ValueTask<bool> DeleteInvoice()
    {
        bool errorTrip = false;

        foreach (var item in InvoiceItems)
        {
            var result = await _broker.DeleteItemAsync<InvoiceItem>(CommandRequest<InvoiceItem>.Create(item));
            if (!result.Successful)
                _logger.LogError($"InvoiceItem - {item.Uid} - {result.Message}");

            errorTrip = !result.Successful | errorTrip;
        }
        {
            var result = await _broker.DeleteItemAsync<Invoice>(CommandRequest<Invoice>.Create(this.Invoice));
            if (!result.Successful)
                _logger.LogError($"InvoiceItem - {this.Invoice.Uid} - {result.Message}");

            errorTrip = !result.Successful | errorTrip;
        }

        return !errorTrip;
    }

    public async ValueTask<bool> SaveInvoice()
    {
        bool errorTrip = false;

        // Create the invoice if it new (defined by it have an empty Guid)
        if (this.Invoice.Uid == Guid.Empty)
        {
            var result = await _broker.CreateItemAsync<Invoice>(CommandRequest<Invoice>.Create(this.Invoice));
            if (!result.Successful)
                _logger.LogError($"Invoice - {this.Invoice.Uid} - {result.Message}");

            errorTrip = !result.Successful | errorTrip;
        }

        // Update the invoice if it already has a Guid)
        if (this.Invoice.Uid != Guid.Empty)
        {
            var result = await _broker.UpdateItemAsync<Invoice>(CommandRequest<Invoice>.Create(this.Invoice));
            if (!result.Successful)
                _logger.LogError($"Invoice - {this.Invoice.Uid} - {result.Message}");

            errorTrip = !result.Successful | errorTrip;
        }

        foreach (var item in InvoiceItems)
        {
            // Create the Invoice Item if it new (defined by it have an empty Guid)
            if (item.Uid == Guid.Empty)
            {
                var result = await _broker.CreateItemAsync<InvoiceItem>(CommandRequest<InvoiceItem>.Create(item));
                if (!result.Successful)
                    _logger.LogError($"Invoice Item - {item.Uid} - {result.Message}");

                errorTrip = !result.Successful | errorTrip;
            }

            // Update the Invoice Item if it already has a Guid)
            if (this.Invoice.Uid != Guid.Empty)
            {
                var result = await _broker.UpdateItemAsync<InvoiceItem>(CommandRequest<InvoiceItem>.Create(item));
                if (!result.Successful)
                    _logger.LogError($"Invoice Item - {item.Uid} - {result.Message}");

                errorTrip = !result.Successful | errorTrip;
            }
        }

        return !errorTrip;
    }
}
