/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.Core;

public sealed record ListQueryRequest
{
    public int StartIndex { get; init; } = 0;
    public int PageSize { get; init; } = 1000;
    public CancellationToken Cancellation { get; set; } = new();
    public bool SortDescending { get; init; } = false;
    //public Expression<Func<TRecord, bool>>? FilterExpression { get; init; }
    //public Expression<Func<TRecord, object>>? SortExpression { get; init; }
    public IEnumerable<FilterDefinition> Filters { get; init; } = Enumerable.Empty<FilterDefinition>();
    public string SortField { get; init; } = string.Empty;

}
