/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.Infrastructure;

public sealed class DeleteRequestBaseServerHandler<TDbContext>
    : IDeleteRequestHandler
    where TDbContext : DbContext
{
    private readonly IDbContextFactory<TDbContext> _factory;

    public DeleteRequestBaseServerHandler(IDbContextFactory<TDbContext> factory)
        => _factory = factory;

    public async ValueTask<CommandResult> ExecuteAsync<TRecord>(CommandRequest<TRecord> request)
        where TRecord : class, new()
    {
        using var dbContext = _factory.CreateDbContext();

        dbContext.Remove<TRecord>(request.Item);
        return await dbContext.SaveChangesAsync(request.Cancellation) == 1
            ? CommandResult.Success("Record Deleted")
            : CommandResult.Failure("Error deleting Record");
    }
}