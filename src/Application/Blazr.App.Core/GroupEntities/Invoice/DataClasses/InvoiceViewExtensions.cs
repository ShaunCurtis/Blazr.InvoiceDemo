/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.App.Core;

public static class InvoiceViewExtensions
{
    public static Invoice ToInvoice(this InvoiceView item)
        => new()
        {
            Uid = item.Uid,
            InvoiceDate= item.InvoiceDate,
            InvoiceNumber= item.InvoiceNumber,
            InvoicePrice= item.InvoicePrice,
            CustomerName= item.CustomerName,
            CustomerUid= item.CustomerUid,
        };
}
