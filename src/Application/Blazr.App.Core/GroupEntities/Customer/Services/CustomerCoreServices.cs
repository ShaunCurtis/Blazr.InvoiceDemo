using Blazr.Core;
/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Core;

public static class CustomerCoreServices
{
    public static void AddCustomerCoreServices(this IServiceCollection services)
    {
        services.AddScoped<IListPresenter<Customer, CustomerEntityService>, ListPresenter<Customer, CustomerEntityService>>();
        services.AddScoped<CustomerEntityService>();
        services.AddScoped<INotificationService<CustomerEntityService>, NotificationService<CustomerEntityService>>();
        services.AddScoped<IForeignKeyPresenter<CustomerFkItem, CustomerEntityService>, ForeignKeyPresenter<CustomerFkItem, CustomerEntityService>>();
        services.AddTransient<IEditPresenter<Customer, CustomerEditContext>, EditPresenter<Customer, CustomerEntityService, CustomerEditContext>>();
        services.AddTransient<IReadPresenter<Customer>, ReadPresenter<Customer>>();
        services.AddTransient<IRecordSorter<Customer>, CustomerSorter>();
        //services.AddTransient<IRecordFilter<Customer>, CustomerFilter>();
        services.AddTransient<IListController<Customer>, ListController<Customer>>();
    }

}
