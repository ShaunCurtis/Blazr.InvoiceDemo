/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.App.Core;

public static class InvoiceItemViewExtensions
{
    public static InvoiceItem ToInvoiceItem(this InvoiceItemView item)
        => new()
        {
            Uid = item.Uid,
            InvoiceUid = item.InvoiceUid,
            ItemQuantity = item.ItemQuantity,
            ItemUnitPrice = item.ItemUnitPrice,
            ProductUid = item.ProductUid,
        };
}
