/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
using Blazr.Infrastructure;
using Microsoft.Extensions.Logging;

namespace Blazr.App.Tests;

public class RepositoryBrokerTests
{
    private InvoiceTestDataProvider _testDataProvider;

    public RepositoryBrokerTests()
        => _testDataProvider = InvoiceTestDataProvider.Instance();

    private ServiceProvider GetServiceProvider()
    {
        var services = new ServiceCollection();
        services.AddAppServerDataServices();
        services.AddLogging(builder => builder.AddDebug());

        var provider = services.BuildServiceProvider();

        // get the DbContext factory and add the test data
        var factory = provider.GetService<IDbContextFactory<InMemoryInvoiceDbContext>>();
        if (factory is not null)
            InvoiceTestDataProvider.Instance().LoadDbContext<InMemoryInvoiceDbContext>(factory);

        return provider!;
    }

    [Fact]
    public async void TestRepositoryDataBrokerInvoiceList()
    {
        var provider = GetServiceProvider();
        var broker = provider.GetService<IDataBroker>()!;

        var cancelToken = new CancellationToken();
        var listRequest = new ListQueryRequest { StartIndex = 0, PageSize = 1000, Cancellation = cancelToken };
        var result = await broker!.GetItemsAsync<Invoice>(listRequest);

        Assert.Equal(3, result.Items.Count());
    }

    [Fact]
    public async void TestRepositoryDataBrokerInvoiceManagerAddItem()
    {
        var provider = GetServiceProvider();
        var broker = provider.GetService<IDataBroker>()!;

        var testUid = _testDataProvider.Invoices.First().Uid;

        var presenter = provider.GetService<InvoiceDataPresenter>()!;

        await presenter.LoadAsync(testUid);
        var items = presenter.Item.InvoiceItems.Count();

        var newItem = _testDataProvider.GetNewInvoiceItem(testUid);

        presenter.Item.AddInvoiceItem(newItem);

        Assert.Equal(testUid, presenter.Item.Invoice.Uid);
        Assert.Equal(items + 1, presenter.Item.InvoiceItems.Count());
    }

    [Fact]
    public async void TestRepositoryDataBrokerInvoiceManagerUpdateItem()
    {
        var provider = GetServiceProvider();
        var broker = provider.GetService<IDataBroker>()!;

        var testUid = _testDataProvider.Invoices.First().Uid;

        var presenter = provider.GetService<InvoiceDataPresenter>()!;

        await presenter.LoadAsync(testUid);
        var items = presenter.Item.InvoiceItems.Count();

        // Get the item to edit and change it
        var editItem = presenter.Item.InvoiceItems.First();
        var testInvoiceItemUid = editItem.Uid;

        // change it and update the invoice
        var editedItem = editItem with { ItemQuantity=5 };
        presenter.Item.UpdateInvoiceItem(editedItem);

        // Save the changes to the database
        await presenter.SaveAsync();

        // Get a new InvoiceNManager instance and load it
        var newPresenter = provider.GetService<InvoiceDataPresenter>()!;
        await newPresenter.LoadAsync(testUid);

        var updatedItem = newPresenter.Item.InvoiceItems.First(item => item.Uid.Equals(testInvoiceItemUid));

        Assert.Equal(editedItem, updatedItem);
        Assert.Equal(items, newPresenter.Item.InvoiceItems.Count());
    }

    [Fact]
    public async void TestRepositoryDataBrokerInvoiceManagerDeleteItem()
    {
        var provider = GetServiceProvider();
        var broker = provider.GetService<IDataBroker>()!;

        // Get the first Invoice from the test provider
        var testUid = _testDataProvider.Invoices.First().Uid;

        // Get a ViewManager instance and load it
        var presenter = provider.GetService<InvoiceDataPresenter>()!;
        await presenter.LoadAsync(testUid);
        var items = presenter.Item.InvoiceItems.Count();

        // Get the item to delete and delete it
        var deleteItem = presenter.Item.InvoiceItems.First();
        presenter.Item.RemoveInvoiceItem(deleteItem);

        // Save the changes to the database
        await presenter.SaveAsync();

        // Get a new InvoiceNManager instance and load it
        var newPresenter = provider.GetService<InvoiceDataPresenter>()!;
        await newPresenter.LoadAsync(testUid);

        Assert.Equal(newPresenter.Item.InvoiceItems.Count(), presenter.Item.InvoiceItems.Count());
    }
}
