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
        //services.AddScoped<ICustomDataBroker, CustomDataBroker>();

        // Add the standard handlers
        services.AddScoped<IListRequestHandler, ListRequestServerHandler<InMemoryInvoiceDbContext>>();
        services.AddScoped<IItemRequestHandler, ItemRequestServerHandler<InMemoryInvoiceDbContext>>();
        services.AddScoped<IUpdateRequestHandler, UpdateRequestServerHandler<InMemoryInvoiceDbContext>>();
        services.AddScoped<ICreateRequestHandler, CreateRequestServerHandler<InMemoryInvoiceDbContext>>();
        services.AddScoped<IDeleteRequestHandler, DeleteRequestServerHandler<InMemoryInvoiceDbContext>>();
        services.AddScoped<ISaveRequestHandler, SaveRequestServerHandler<InMemoryInvoiceDbContext>>();

        // Add the base handlers
        services.AddScoped<ListRequestBaseServerHandler<InMemoryInvoiceDbContext>>();
        services.AddScoped<ItemRequestBaseServerHandler<InMemoryInvoiceDbContext>>();
        services.AddScoped<UpdateRequestBaseServerHandler<InMemoryInvoiceDbContext>>();
        services.AddScoped<CreateRequestBaseServerHandler<InMemoryInvoiceDbContext>>();
        services.AddScoped<DeleteRequestBaseServerHandler<InMemoryInvoiceDbContext>>();
        services.AddScoped<SaveRequestBaseServerHandler<InMemoryInvoiceDbContext>>();

        // Add custom handlers
        services.AddScoped<IItemRequestHandler<Customer>, CustomerRequestServerHandler<InMemoryInvoiceDbContext>>();

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
