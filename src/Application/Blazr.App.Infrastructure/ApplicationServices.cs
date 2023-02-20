/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Infrastructure;

public static class ApplicationServices
{
    public static void AddAppServerDataServices(this IServiceCollection services)
        => services.AddAppServerDataServices<InMemoryInvoiceDbContext>(options
            => options.UseInMemoryDatabase($"InvoiceDatabase-{Guid.NewGuid().ToString()}"));

    public static void AddAppTestDataServices(this IServiceCollection services)
        => services.AddDbContextFactory<InMemoryInvoiceDbContext>(options
            => options.UseInMemoryDatabase($"InvoiceDatabase-{Guid.NewGuid().ToString()}"));

    public static void AddAppServerDataServices<TDbContext>(this IServiceCollection services, Action<DbContextOptionsBuilder> options) where TDbContext : DbContext
    {
        AddAppServerInfrastructureServices<TDbContext>(services, options);
        AddAppDataServices(services);
    }

    public static void AddAppWASMDataServices(this IServiceCollection services)
    {
        AddAppWASMInfraStructureServices(services);
        AddAppDataServices(services);
    }

    public static void AddAppAPIServerDataServices<TDbContext>(this IServiceCollection services, Action<DbContextOptionsBuilder> options) where TDbContext : DbContext
    {
        AddAppWASMInfraStructureServices(services);
    }

    private static void AddAppServerInfrastructureServices<TDbContext>(this IServiceCollection services, Action<DbContextOptionsBuilder> options) where TDbContext : DbContext
    {
        services.AddDbContextFactory<TDbContext>(options);
        services.AddScoped<IDataBroker, RepositoryDataBroker>();
        services.AddScoped<ICustomDataBroker, CustomDataBroker>();

        services.AddScoped<IListRequestHandler, ListRequestHandler<InMemoryInvoiceDbContext>>();
        services.AddScoped<IItemRequestHandler, ItemRequestHandler<InMemoryInvoiceDbContext>>();
        services.AddScoped<IUpdateRequestHandler, UpdateRequestHandler<InMemoryInvoiceDbContext>>();
        services.AddScoped<ICreateRequestHandler, CreateRequestHandler<InMemoryInvoiceDbContext>>();
        services.AddScoped<IDeleteRequestHandler, DeleteRequestHandler<InMemoryInvoiceDbContext>>();

        services.AddInvoiceServerInfrastructureServices();
    }

    private static void AddAppWASMInfraStructureServices(this IServiceCollection services)
    {
        services.AddScoped<IDataBroker, RepositoryDataBroker>();

        services.AddScoped<IListRequestHandler, ListRequestAPIHandler>();
        services.AddScoped<IItemRequestHandler, ItemRequestAPIHandler>();
        services.AddScoped<IUpdateRequestHandler, UpdateRequestAPIHandler>();
        services.AddScoped<ICreateRequestHandler, CreateRequestAPIHandler>();
        services.AddScoped<IDeleteRequestHandler, DeleteRequestAPIHandler>();
    }

    private static void AddAppDataServices(this IServiceCollection services)
    {
        services.AddCustomerCoreServices();
        services.AddProductCoreServices();
        services.AddInvoiceCoreServices();
        services.AddInvoiceItemCoreServices();
    }

    public static void AddTestData(IServiceProvider provider)
    {
        var factory = provider.GetService<IDbContextFactory<InMemoryInvoiceDbContext>>();

        if (factory is not null)
            InvoiceTestDataProvider.Instance().LoadDbContext<InMemoryInvoiceDbContext>(factory);
    }

}
