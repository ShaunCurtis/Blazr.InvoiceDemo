/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.App.Core;

public class InvoiceItemFilter : IRecordFilter<InvoiceItem>
{
    public IPredicateSpecification<InvoiceItem>? GetSpecification(FilterDefinition filter)
    => filter.FilterName switch
    {
        ApplicationConstants.ByInvoiceUid => new InvoiceItemsByInvoiceUidSpecification(filter),
        ApplicationConstants.ByProductUid => new InvoiceItemsByProductUidSpecification(filter),
        _ => null
    };
}
