/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.App.Infrastructure;

public sealed class InvoiceDataItemHandler<TDbContext> : 
    IItemRequestHandler<InvoiceAggregate>
    where TDbContext : DbContext
{
    private readonly IDbContextFactory<TDbContext> _factory;
    private ILogger<InvoiceDataItemHandler<TDbContext>> _logger;

    public InvoiceDataItemHandler(IDbContextFactory<TDbContext> factory, ILogger<InvoiceDataItemHandler<TDbContext>> logger)
    {
        _factory = factory;
        _logger = logger;
    }

    public async ValueTask<ItemQueryResult<InvoiceAggregate>> ExecuteAsync(ItemQueryRequest request)
    {
        if (request == null)
            throw new DataPipelineException($"No CommandRequest defined in {this.GetType().FullName}");

        using var dbContext = _factory.CreateDbContext();

        var invoice = await dbContext.Set<Invoice>().FirstOrDefaultAsync( item => item.Uid == request.Uid); 

        // Check if we got an invoice or have defaulted to a new invoice
        if (invoice is null)
            return ItemQueryResult<InvoiceAggregate>.Failure("Could Not find requested Invoice");


        // Get the items associated with the invoice
        var invoiceItems = await dbContext.Set<InvoiceItem>()
            .Where(item => item.InvoiceUid == request.Uid)
            .ToListAsync();

        if (invoiceItems is not null)
            return ItemQueryResult<InvoiceAggregate>.Success(new(invoice, invoiceItems));

        return ItemQueryResult<InvoiceAggregate>.Failure("Could Not fufill the request.");
    }
}
