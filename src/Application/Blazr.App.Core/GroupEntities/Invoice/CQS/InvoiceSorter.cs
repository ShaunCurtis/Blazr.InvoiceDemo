/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
using System.Linq.Expressions;

namespace Blazr.App.Core;

public class InvoiceSorter : RecordSortBase<Invoice>, IRecordSorter<Invoice>
{
    public IQueryable<Invoice> AddSortToQuery(string fieldName, IQueryable<Invoice> query, bool sortDescending)
        => fieldName switch
        {
            ApplicationConstants.InvoiceNumber => Sort(query, sortDescending, OnInvoiceNumber),
            _ => Sort(query, sortDescending, OnInvoiceDate)
        };

private static Expression<Func<Invoice, object>> OnInvoiceDate => item => item.InvoiceDate;
private static Expression<Func<Invoice, object>> OnInvoiceNumber => item => item.InvoiceNumber ?? string.Empty;
}
