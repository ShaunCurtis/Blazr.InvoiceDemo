/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.App.Infrastructure;

internal static class InvoiceViewExtensions
{
    internal static DboInvoice ToDboInvoice(this InvoiceView item)
        => new()
        {
            Uid = item.Uid,
            InvoiceDate= item.InvoiceDate,
            InvoiceNumber= item.InvoiceNumber,
            InvoicePrice= item.InvoicePrice,
            CustomerUid= item.CustomerUid,
        };
}
