/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Core;

public static class InvoiceItemCoreServices
{
    public static void AddInvoiceItemCoreServices(this IServiceCollection services)
    {
        services.AddScoped<IListPresenter<InvoiceItem, InvoiceItemEntityService>, ListPresenter<InvoiceItem, InvoiceItemEntityService>>();
        services.AddScoped<IListPresenter<InvoiceItemView, InvoiceItemEntityService>, ListPresenter<InvoiceItemView, InvoiceItemEntityService>>();
        services.AddScoped<InvoiceItemEntityService>();
        services.AddScoped<INotificationService<InvoiceItemEntityService>, NotificationService<InvoiceItemEntityService>>();
        services.AddTransient<IEditPresenter<InvoiceItem, InvoiceItemEditContext>, EditPresenter<InvoiceItem, InvoiceItemEntityService, InvoiceItemEditContext>>();
        services.AddTransient<IReadPresenter<InvoiceItem>, ReadPresenter<InvoiceItem>>();
        services.AddTransient<IReadPresenter<InvoiceItemView>, ReadPresenter<InvoiceItemView>>();
        services.AddTransient<IRecordSorter<InvoiceItem>, InvoiceItemSorter>();
        services.AddTransient<IRecordFilter<InvoiceItem>, InvoiceItemFilter>();
        services.AddTransient<ListController<InvoiceItem>>();
        services.AddTransient<ListController<InvoiceItemView>>();
    }

}
