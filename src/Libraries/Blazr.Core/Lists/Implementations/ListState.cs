/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
using System.Diagnostics;
using System.Reflection;

namespace Blazr.Core;

public class ListState<TRecord>
    where TRecord : class, new()
{
    public string? SortField { get; private set; }

    public bool SortDescending { get; private set; }

    public int PageSize { get; private set; } = 1000;

    public int StartIndex { get; private set; } = 0;

    public int ListTotalCount { get; private set; } = 0;

    public Expression<Func<TRecord, bool>>? FilterExpression { get; private set; }
    public IEnumerable<FilterDefinition> Filters { get; private set; } = Enumerable.Empty<FilterDefinition>();

    public Expression<Func<TRecord, object>>? SortExpression { get; private set; }

    public int Page => StartIndex / PageSize;

    public void SetFromListState(ListState<TRecord> state)
    {
        this.SortField = state.SortField;
        this.SortDescending = state.SortDescending;
        this.PageSize = state.PageSize;
        this.StartIndex = state.StartIndex;
        this.ListTotalCount = state.ListTotalCount;
        this.Filters = state.Filters;
        this.SortField = state.SortField;
    }

    public void SetSorting(SortRequest request)
    {
        this.SortField = request.SortField;
        this.SortDescending = request.SortDescending;
    }

    public void SetPaging(PagingRequest rquest)
    {
        this.StartIndex = rquest.StartIndex;
        this.PageSize = rquest.PageSize;
    }

    //TODO needs fixing
    public void SetFiltering(FilterRequest<TRecord> request)
        => this.Filters = request.Filters;

    public void SetPaging(int startIndex, int pageSize, IEnumerable<FilterDefinition>? filters = null)
    {
        this.StartIndex = startIndex;
        this.PageSize = pageSize;
        this.Filters = filters ?? Enumerable.Empty<FilterDefinition>();
    }

    public void SetPaging(int startIndex, IEnumerable<FilterDefinition>? filters = null)
    {
        this.StartIndex = startIndex;
        this.Filters = filters ?? Enumerable.Empty<FilterDefinition>();
    }

    public void Set(PagingRequest paging, SortRequest sorting, IEnumerable<FilterDefinition>? filters = null)
    {
        this.SetPaging(paging);
        this.SetSorting(sorting);
        this.Filters = filters ?? Enumerable.Empty<FilterDefinition>();
    }

    public void Set(ListQueryRequest request, ListQueryResult<TRecord> result)
    {
        this.PageSize = request.PageSize;
        this.StartIndex = request.StartIndex;
        this.ListTotalCount = result.TotalCount > int.MaxValue ? int.MaxValue : (int)result.TotalCount;
    }

    public PagingRequest GetAsPagingRequest()
        => new PagingRequest() { PageSize = this.PageSize, StartIndex = this.StartIndex };

    public ListQueryRequest GetListQueryRequest()
    {
        return new ListQueryRequest
        {
            PageSize = this.PageSize,
            StartIndex = this.StartIndex,
            SortDescending = this.SortDescending,
            Filters = this.Filters,
            SortField = this.SortField ?? string.Empty
        };
    }
}
 
