/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;

namespace Blazr.App.Core;

public class InvoiceItemFilter : IRecordFilter<InvoiceItem>
{
    public IQueryable<InvoiceItem> AddFilterToQuery(IEnumerable<FilterDefinition> filters, IQueryable<InvoiceItem> query)
    {
        foreach (var filter in filters)
        {
            switch (filter.FilterName)
            {
                case ApplicationConstants.ByInvoiceUid:
                    if (BindConverter.TryConvertTo<Guid>(filter.FilterData, null, out Guid uidValue))
                        query = query.Where(ByInvoiceUid(uidValue));
                    break;

                case ApplicationConstants.ByProductUid:
                    if (BindConverter.TryConvertTo<Guid>(filter.FilterData, null, out Guid uidProductValue))
                        query = query.Where(ByProductUid(uidProductValue));
                    break;

                default:
                    break;
            }
        }

        if (query is IQueryable)
            return query;

        return query.AsQueryable();
    }

    private static Expression<Func<InvoiceItem, bool>> ByInvoiceUid(Guid value) => item => item.InvoiceUid.Equals(value);
    private static Expression<Func<InvoiceItem, bool>> ByProductUid(Guid value) => item => item.ProductUid.Equals(value);
}
