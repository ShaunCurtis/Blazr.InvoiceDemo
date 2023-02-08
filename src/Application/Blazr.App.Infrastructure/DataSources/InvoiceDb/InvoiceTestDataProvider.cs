/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Infrastructure;

/// <summary>
/// A class to build a fixed data set for testing
/// </summary>
public sealed class InvoiceTestDataProvider
{
    public IEnumerable<Product> Products => _products ?? Enumerable.Empty<Product>();
    public IEnumerable<Customer> Customers => _customers ?? Enumerable.Empty<Customer>();
    public IEnumerable<Invoice> Invoices => _invoices ?? Enumerable.Empty<Invoice>();
    public IEnumerable<InvoiceItem> InvoiceItems => _invoiceItems ?? Enumerable.Empty<InvoiceItem>();

    private List<Product>? _products;
    private List<Customer>? _customers;
    private List<Invoice>? _invoices;
    private List<InvoiceItem>? _invoiceItems;

    private InvoiceTestDataProvider()
        => this.Load();

    public void LoadDbContext<TDbContext>(IDbContextFactory<TDbContext> factory) where TDbContext : DbContext
    {
        using var dbContext = factory.CreateDbContext();

        var products = dbContext.Set<Product>();

        // Check if we already have a full data set
        // If not clear down any existing data and start again
        if (products.Count() == 0)
        {
            dbContext.AddRange(this.Products);
            dbContext.SaveChanges();
        }

        var customers = dbContext.Set<Customer>();

        // Check if we already have a full data set
        // If not clear down any existing data and start again
        if (customers.Count() == 0)
        {
            dbContext.AddRange(this.Customers);
            dbContext.SaveChanges();
        }

        var invoices = dbContext.Set<Invoice>();

        // Check if we already have a full data set
        // If not clear down any existing data and start again
        if (invoices.Count() == 0)
        {
            dbContext.AddRange(this.Invoices);
            dbContext.SaveChanges();
        }

        var invoiceItems = dbContext.Set<InvoiceItem>();

        // Check if we already have a full data set
        // If not clear down any existing data and start again
        if (invoiceItems.Count() == 0)
        {
            dbContext.AddRange(this.InvoiceItems);
            dbContext.SaveChanges();
        }
    }

    public void Load()
    {
        LoadProducts();
        LoadCustomers();
        LoadInvoices();
        LoadInvoiceItems();
    }

    private void LoadProducts()
    {
        var products = new List<Product>()
        {
            new() { Uid=Guid.NewGuid(), ProductCode="SKU100", ProductName="Boeing 707", ProductUnitPrice=12.50m },
            new() { Uid=Guid.NewGuid(), ProductCode="SKU101", ProductName="Boeing 727", ProductUnitPrice=13.50m },
            new() { Uid=Guid.NewGuid(), ProductCode="SKU102", ProductName="Boeing 737", ProductUnitPrice=14.50m },
            new() { Uid=Guid.NewGuid(), ProductCode="SKU103", ProductName="Boeing 747", ProductUnitPrice=19.50m },
            new() { Uid=Guid.NewGuid(), ProductCode="SKU104", ProductName="Boeing 757", ProductUnitPrice=25.50m },
            new() { Uid=Guid.NewGuid(), ProductCode="SKU105", ProductName="Boeing 767", ProductUnitPrice=26.50m },
            new() { Uid=Guid.NewGuid(), ProductCode="SKU106", ProductName="Boeing 777", ProductUnitPrice=31.50m },
            new() { Uid=Guid.NewGuid(), ProductCode="SKU107", ProductName="Boeing 787", ProductUnitPrice=32.50m },
            new() { Uid=Guid.NewGuid(), ProductCode="SKU110", ProductName="Airbus A300", ProductUnitPrice=12.50m },
            new() { Uid=Guid.NewGuid(), ProductCode="SKU111", ProductName="Airbus A310", ProductUnitPrice=13.50m },
            new() { Uid=Guid.NewGuid(), ProductCode="SKU112", ProductName="Airbus A319", ProductUnitPrice=14.50m },
            new() { Uid=Guid.NewGuid(), ProductCode="SKU113", ProductName="Airbus A320", ProductUnitPrice=15.50m },
            new() { Uid=Guid.NewGuid(), ProductCode="SKU114", ProductName="Airbus A321", ProductUnitPrice=16.50m },
            new() { Uid=Guid.NewGuid(), ProductCode="SKU115", ProductName="Airbus A330", ProductUnitPrice=17.50m },
            new() { Uid=Guid.NewGuid(), ProductCode="SKU116", ProductName="Airbus A340", ProductUnitPrice=22.50m },
            new() { Uid=Guid.NewGuid(), ProductCode="SKU117", ProductName="Airbus A350", ProductUnitPrice=23.50m },
            new() { Uid=Guid.NewGuid(), ProductCode="SKU118", ProductName="Airbus A380", ProductUnitPrice=25.50m },
            new() { Uid=Guid.NewGuid(), ProductCode="SKU119", ProductName="Airbus A220", ProductUnitPrice=37.50m },
            new() { Uid=Guid.NewGuid(), ProductCode="SKU220", ProductName="Fokker F27", ProductUnitPrice=2.50m },
            new() { Uid=Guid.NewGuid(), ProductCode="SKU221", ProductName="Fokker F28", ProductUnitPrice=3.50m },
            new() { Uid=Guid.NewGuid(), ProductCode="SKU321", ProductName="BAE 146", ProductUnitPrice=4.50m },
            new() { Uid=Guid.NewGuid(), ProductCode="SKU323", ProductName="BAE ATR", ProductUnitPrice=3.50m },
            new() { Uid=Guid.NewGuid(), ProductCode="SKU401", ProductName="Tupolev 204", ProductUnitPrice=7.50m },
            new() { Uid=Guid.NewGuid(), ProductCode="SKU402", ProductName="Tupolev 214", ProductUnitPrice=4.50m },
        };
        _products = products;
    }


    private void LoadCustomers()
    {
        var customers = new List<Customer>()
        {
            new() { Uid=Guid.NewGuid(), CustomerName="EasyJet"},
            new() { Uid=Guid.NewGuid(), CustomerName="RyanAir"},
            new() { Uid=Guid.NewGuid(), CustomerName="Air France"},
            new() { Uid=Guid.NewGuid(), CustomerName="TAP"},
        };
        _customers = customers;
    }

    private void LoadInvoices()
    {
        var invoices = new List<Invoice>()
        {
            new() { Uid=Guid.NewGuid(), CustomerUid = _customers![Random.Shared.Next(Customers.Count())].Uid, InvoiceDate = DateOnly.FromDateTime(DateTime.Now), InvoiceNumber="1001", InvoicePrice=1000m},
            new() { Uid=Guid.NewGuid(), CustomerUid = _customers![Random.Shared.Next(Customers.Count())].Uid, InvoiceDate = DateOnly.FromDateTime(DateTime.Now), InvoiceNumber="1002", InvoicePrice=2000m},
            new() { Uid=Guid.NewGuid(), CustomerUid = _customers![Random.Shared.Next(Customers.Count())].Uid, InvoiceDate = DateOnly.FromDateTime(DateTime.Now), InvoiceNumber="1003", InvoicePrice=3000m},
        };
        _invoices = invoices;
    }

    private void LoadInvoiceItems()
    {
        var invoiceItems = new List<InvoiceItem>();

        foreach (var invoice in this.Invoices)
        {
            var product = _products![Random.Shared.Next(_products.Count())];
            invoiceItems.Add(new() { InvoiceUid = invoice.Uid, ProductUid = product.Uid, ItemQuantity = Random.Shared.Next(4), ItemUnitPrice = product.ProductUnitPrice });
            invoiceItems.Add(new() { InvoiceUid = invoice.Uid, ProductUid = product.Uid, ItemQuantity = Random.Shared.Next(4), ItemUnitPrice = product.ProductUnitPrice });
        }

        _invoiceItems = invoiceItems;
    }

    private static InvoiceTestDataProvider? _provider;

    public static InvoiceTestDataProvider Instance()
    {
        if (_provider is null)
            _provider = new InvoiceTestDataProvider();

        return _provider;
    }
}
