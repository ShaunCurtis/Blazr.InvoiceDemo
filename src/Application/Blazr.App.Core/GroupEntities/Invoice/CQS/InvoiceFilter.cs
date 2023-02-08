/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.App.Core;

public class InvoiceFilter : IRecordFilter<Invoice>
{
    public IPredicateExpressionSpecification<Invoice>? GetSpecification(FilterDefinition filter)
        => filter.FilterName switch
        {
            ApplicationConstants.ByCustomerUid => new InvoicesByCustomerUidSpecification(filter),
            ApplicationConstants.ByInvoiceMonth => new InvoicesByMonthSpecification(filter),
            _ => null
        };
}
