/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Core;

public static class InvoiceCoreServices
{
    public static void AddInvoiceCoreServices(this IServiceCollection services)
    {
        services.AddScoped<IListPresenter<Invoice, InvoiceEntityService>, ListPresenter<Invoice, InvoiceEntityService>>();
        services.AddScoped<InvoiceEntityService>();
        services.AddScoped<INotificationService<InvoiceEntityService>, NotificationService<InvoiceEntityService>>();
        services.AddTransient<IEditPresenter<Invoice, InvoiceEditContext>, EditPresenter<Invoice, InvoiceEntityService, InvoiceEditContext>>();
        services.AddTransient<IReadPresenter<Invoice>, ReadPresenter<Invoice>>();
        services.AddTransient<IRecordSorter<Invoice>, InvoiceSorter>();
        services.AddTransient<IRecordFilter<Invoice>, InvoiceFilter>();
        services.AddTransient<ListController<Invoice>>();
    }

}
