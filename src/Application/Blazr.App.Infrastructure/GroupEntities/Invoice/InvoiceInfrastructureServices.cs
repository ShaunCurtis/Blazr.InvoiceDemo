/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Infrastructure;

public static class InvoiceCoreServices
{
    public static void AddInvoiceServerInfrastructureServices(this IServiceCollection services)
    {
        services.AddTransient<IItemRequestHandler<InvoiceData>, InvoiceDataItemHandler>();
        services.AddTransient<IDeleteRequestHandler<InvoiceData>, InvoiceDataDeleteHandler>();
        services.AddTransient<ISaveRequestHandler<InvoiceData>, InvoiceDataSaveHandler>();
    }
}
