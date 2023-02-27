/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.App.Infrastructure;

public sealed class InvoiceDataDeleteHandler<TDbContext>
    : IDeleteRequestHandler<InvoiceAggregate>
    where TDbContext : DbContext
{
    private readonly IDbContextFactory<TDbContext> _factory;
    private ILogger<InvoiceDataItemHandler<TDbContext>> _logger;

    public InvoiceDataDeleteHandler(IDbContextFactory<TDbContext> factory, ILogger<InvoiceDataItemHandler<TDbContext>> logger)
    {
        _factory = factory;
        _logger = logger;
    }

    public async ValueTask<CommandResult> ExecuteAsync(CommandRequest<InvoiceAggregate> request)
    {
        if (request == null)
        {
            var message = $"No Delete CommandRequest defined in {this.GetType().FullName}";
            _logger.LogError(message);
            throw new DataPipelineException(message);
        }

        using var dbContext = _factory.CreateDbContext();

        var invoiceData = request.Item;

        foreach (var item in request.Item.InvoiceItems)
            dbContext.Remove<DboInvoiceItem>(item.ToDboInvoiceItem());

        dbContext.Remove<DboInvoice>(invoiceData.Invoice.ToDboInvoice());

        try
        {
            var transactions = await dbContext.SaveChangesAsync();
            return CommandResult.Success();
        }
        catch (DbUpdateException)
        {
            var message = $"Failed to delete the invoice {request.Item.Uid}.  Transaction aborted";
            _logger.LogError(message);
            return CommandResult.Failure(message);
        }
        catch (Exception e)
        {
            var message = $"An error occured trying to delete invoice {request.Item.Uid}.  Detail: {e.Message}.";
            _logger.LogError(message);
            return CommandResult.Failure(message);
        }
    }
}
