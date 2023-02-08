/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.UI;

public static class ApplicationUIServices
{
    public static void AddAppUIServices(this IServiceCollection services)
    {
        services.AddScoped<IUiStateService, UiStateService>();
        
        services.AddSingleton<IUIEntityService<ProductEntityService>, ProductUIEntityService>();
        services.AddSingleton<IUIEntityService<CustomerEntityService>, CustomerUIEntityService>();
        services.AddSingleton<IUIEntityService<InvoiceEntityService>, InvoiceUIEntityService>();
        services.AddSingleton<IUIEntityService<InvoiceItemEntityService>, InvoiceItemUIEntityService>();
    }
}
