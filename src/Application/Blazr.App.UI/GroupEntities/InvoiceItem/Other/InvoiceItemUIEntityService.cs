/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
 
namespace Blazr.App.UI;

public sealed record InvoiceItemUIEntityService : IUIEntityService<InvoiceItemEntityService>
{
    public string SingleDisplayName { get; } = "Invoice Item";
    public string PluralDisplayName { get; } = "Invoice Items";
    public Type? EditForm { get; } = typeof(CustomerEditForm);
    public Type? ViewForm { get; } = typeof(CustomerViewForm);
    public string Url { get; } = "/invoiceitem";
}
