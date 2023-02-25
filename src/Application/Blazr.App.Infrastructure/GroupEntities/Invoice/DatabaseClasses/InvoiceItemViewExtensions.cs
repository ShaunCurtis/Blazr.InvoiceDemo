/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.App.Infrastructure;

internal static class InvoiceItemViewExtensions
{
    internal static DboInvoiceItem ToDboInvoiceItem(this InvoiceItem item)
        => new()
        {
            Uid = item.Uid,
            InvoiceUid = item.InvoiceUid,
            ItemQuantity = item.ItemQuantity,
            ItemUnitPrice = item.ItemUnitPrice,
            ProductUid = item.ProductUid,
        };
}
