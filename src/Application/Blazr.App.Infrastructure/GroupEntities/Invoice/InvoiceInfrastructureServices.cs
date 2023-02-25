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
        services.AddTransient<IItemRequestHandler<InvoiceAggregate>, InvoiceDataItemHandler>();
        services.AddTransient<IDeleteRequestHandler<InvoiceAggregate>, InvoiceDataDeleteHandler>();
        services.AddTransient<ISaveRequestHandler<InvoiceAggregate>, InvoiceDataSaveHandler>();
    }
}
