/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;

namespace Blazr.App.Core;

public class InvoiceFilter : IRecordFilter<Invoice>
{
    public IQueryable<Invoice> AddFilterToQuery(IEnumerable<FilterDefinition> filters, IQueryable<Invoice> query)
    {
        foreach (var filter in filters)
        {
            switch (filter.FilterName)
            {
                case ApplicationConstants.ByInvoiceDate:
                    if (BindConverter.TryConvertTo<DateOnly>(filter.FilterData, null, out DateOnly dateValue))
                        query = query.Where(ByInvoiceDate(dateValue));
                    break;

                case ApplicationConstants.ByCustomerUid:
                    if (BindConverter.TryConvertTo<Guid>(filter.FilterData, null, out Guid uidValue))
                        query = query.Where(ByCustomerUid(uidValue));
                    break;

                default:
                    break;
            }
        }

        if (query is IQueryable)
            return query;

        return query.AsQueryable();
    }

    private static Expression<Func<Invoice, bool>> ByCustomerUid(Guid value) => item => item.CustomerUid.Equals(value);
    private static Expression<Func<Invoice, bool>> ByInvoiceDate(DateOnly value) => item => item.InvoiceDate == value;
}
