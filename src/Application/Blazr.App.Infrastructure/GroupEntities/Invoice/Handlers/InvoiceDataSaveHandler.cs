/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.App.Infrastructure;

public sealed class InvoiceDataSaveHandler<TDbContext> 
    : ISaveRequestHandler<InvoiceAggregate>
    where TDbContext : DbContext
{
    private readonly IDbContextFactory<TDbContext> _factory;
    private ILogger<InvoiceDataItemHandler<TDbContext>> _logger;

    public InvoiceDataSaveHandler(IDbContextFactory<TDbContext> factory, ILogger<InvoiceDataItemHandler<TDbContext>> logger)
    {
        _factory = factory;
        _logger = logger;
    }

    public async ValueTask<CommandResult> ExecuteAsync(CommandRequest<InvoiceAggregate> request)
    {
        if (request == null)
        {
            var message = $"No Save CommandRequest defined in {this.GetType().FullName}";
            _logger.LogError(message);
            throw new DataPipelineException(message);
        }

        using var dbContext = _factory.CreateDbContext();

        var invoiceData = request.Item;
        // Create the invoice if it's new
        if (invoiceData.IsNewInvoice)
            dbContext.Add<DboInvoice>(invoiceData.Invoice.ToDboInvoice());

        // Update the invoice if it is dirty)
        if (!invoiceData.IsNewInvoice && invoiceData.InvoiceIsDirty)
            dbContext.Update<DboInvoice>(invoiceData.Invoice.ToDboInvoice());

        // Update all the existing items
        foreach (var item in invoiceData.UpdatedItems)
            dbContext.Update<DboInvoiceItem>(item.ToDboInvoiceItem());

        // Add all the new items
        foreach (var item in invoiceData.AddedItems)
            dbContext.Add<DboInvoiceItem>(item.ToDboInvoiceItem());

        // Remove all the items in the deleted collection
        foreach (var item in invoiceData.DeletedItems)
            dbContext.Remove<DboInvoiceItem>(item.ToDboInvoiceItem());

        try
        {
            var transactions = await dbContext.SaveChangesAsync();
            return CommandResult.Success();
        }
        catch (DbUpdateException)
        {
            var message = $"Failed to save the invoice {request.Item.Uid}.  Transaction aborted";
            _logger.LogError(message);
            return CommandResult.Failure(message);
        }
        catch (Exception e)
        {
            var message = $"An error occured trying to save invoice {request.Item.Uid}.  Detail: {e.Message}.";
            _logger.LogError(message);
            return CommandResult.Failure(message);
        }
    }
}
