/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.App.Core;

public class InvoiceItemSorter : RecordSorter<InvoiceItem>, IRecordSorter<InvoiceItem>
{
    protected override Expression<Func<InvoiceItem, object>> DefaultSorter => item => item.ItemUnitPrice;
}

public class InvoiceItemViewSorter : RecordSorter<InvoiceItemView>, IRecordSorter<InvoiceItemView>
{
    protected override Expression<Func<InvoiceItemView, object>> DefaultSorter => item => item.ProductName;
}
