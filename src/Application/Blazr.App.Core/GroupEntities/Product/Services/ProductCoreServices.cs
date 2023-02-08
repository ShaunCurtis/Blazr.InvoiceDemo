/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Core;

public static class ProductServices
{
    public static void AddProductCoreServices(this IServiceCollection services)
    {
        services.AddScoped<IListPresenter<Product, ProductEntityService>, ListPresenter<Product, ProductEntityService>>();
        services.AddScoped<ProductEntityService>();
        services.AddScoped<INotificationService<ProductEntityService>, NotificationService<ProductEntityService>>();
        services.AddTransient<IEditPresenter<Product, ProductEditContext>, EditPresenter<Product, ProductEntityService, ProductEditContext>>();
        services.AddTransient<IReadPresenter<Product>, ReadPresenter<Product>>();
        services.AddTransient<IRecordSorter<Product>, ProductSorter>();
        //services.AddTransient<IRecordFilter<Product>, ProductFilter>();
        services.AddTransient<ListController<Product>>();
    }
}
