/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
using System.Linq.Expressions;

namespace Blazr.App.Core;

public class InvoiceItemSorter : RecordSortBase<InvoiceItem>, IRecordSorter<InvoiceItem>
{
    public IQueryable<InvoiceItem> AddSortToQuery(string fieldName, IQueryable<InvoiceItem> query, bool sortDescending)
        => fieldName switch
        {
            ApplicationConstants.InvoiceNumber => Sort(query, sortDescending, OnItemQuantity),
            _ => Sort(query, sortDescending, OnItemQuantity)
        };

    private static Expression<Func<InvoiceItem, object>> OnItemQuantity => item => item.ItemQuantity;
    private static Expression<Func<InvoiceItem, object>> OnItemPrice => item => item.ItemUnitPrice;
}
