/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Infrastructure;

public static class ApplicationServices
{
    public static void AddAppServerDataServices(this IServiceCollection services)
        => services.AddAppServerDataServices<InMemoryWeatherDbContext>(options
            => options.UseInMemoryDatabase($"WeatherDatabase-{Guid.NewGuid().ToString()}"));

    public static void AddAppTestDataServices(this IServiceCollection services)
        => services.AddDbContextFactory<InMemoryWeatherDbContext>(options
            => options.UseInMemoryDatabase($"WeatherDatabase-{Guid.NewGuid().ToString()}"));

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

        services.AddScoped<IListRequestHandler, ListRequestHandler<InMemoryWeatherDbContext>>();
        services.AddScoped<IItemRequestHandler, ItemRequestHandler<InMemoryWeatherDbContext>>();
        services.AddScoped<IUpdateRequestHandler, UpdateRequestHandler<InMemoryWeatherDbContext>>();
        services.AddScoped<ICreateRequestHandler, CreateRequestHandler<InMemoryWeatherDbContext>>();
        services.AddScoped<IDeleteRequestHandler, DeleteRequestHandler<InMemoryWeatherDbContext>>();
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
        services.AddScoped<IListPresenter<WeatherForecast, WeatherForecastEntityService>, ListPresenter<WeatherForecast, WeatherForecastEntityService>>();
        services.AddScoped<WeatherForecastEntityService>();
        services.AddScoped<INotificationService<WeatherForecastEntityService>, NotificationService<WeatherForecastEntityService>>();
        services.AddTransient<IEditPresenter<WeatherForecast, WeatherForecastEditContext>, EditPresenter<WeatherForecast, WeatherForecastEntityService, WeatherForecastEditContext>>();
        services.AddTransient<IReadPresenter<WeatherForecast>, ReadPresenter<WeatherForecast>>();
        services.AddTransient<IRecordSorter<WeatherForecast>, WeatherForecastSorter>();
        services.AddTransient<IRecordFilter<WeatherForecast>, WeatherForecastFilter>();
        services.AddTransient<ListController<WeatherForecast>>();

        //TODO probably don't need this anymore
        services.AddScoped<IComponentServiceProvider, ComponentServiceProvider>();

        //TODO - probably don't need these anymore
        services.AddTransient<WeatherSummaryPresenter>();
    }

    public static void AddTestData(IServiceProvider provider)
    {
        var factory = provider.GetService<IDbContextFactory<InMemoryWeatherDbContext>>();

        if (factory is not null)
            WeatherTestDataProvider.Instance().LoadDbContext<InMemoryWeatherDbContext>(factory);
    }

}
