﻿/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.Infrastructure;

public sealed class InMemoryInvoiceDbContext
    : DbContext
{
    public DbSet<Customer> Customer { get; set; } = default!;
    public DbSet<Product> Product { get; set; } = default!;
    public DbSet<Invoice> Invoice { get; set; } = default!;
    public DbSet<InvoiceItem> InvoiceItem { get; set; } = default!;
    public DbSet<InvoiceView> InvoiceView { get; set; } = default!;
    public DbSet<InvoiceItemView> InvoiceItemView { get; set; } = default!;

    public InMemoryInvoiceDbContext(DbContextOptions<InMemoryWeatherDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>().ToTable("Customer");
        modelBuilder.Entity<Product>().ToTable("Product");
        modelBuilder.Entity<Invoice>().ToTable("Invoice");
        modelBuilder.Entity<InvoiceItem>().ToTable("InvoiceItem");

        modelBuilder.Entity<InvoiceView>()
            .ToInMemoryQuery(()
                => from i in this.Invoice
                   join c in this.Customer! on i.CustomerUid equals c.Uid
                   select new InvoiceView
                   {
                       Uid = i.Uid,
                       CustomerUid = i.CustomerUid,
                       CustomerName = c.CustomerName,
                       InvoiceDate = i.InvoiceDate,
                       InvoiceNumber = i.InvoiceNumber,
                   }).HasKey(x => x.Uid);

        modelBuilder.Entity<InvoiceItemView>()
            .ToInMemoryQuery(()
                => from i in this.InvoiceItem
                   join p in this.Product! on i.ProductUid equals p.Uid
                   select new InvoiceItemView
                   {
                       Uid = i.Uid,
                       InvoiceUid= i.InvoiceUid,
                       ProductUid = i.ProductUid,
                       ProductName = p.ProductName,
                       ProductCode = p.ProductCode,
                       ItemQuantity = i.ItemQuantity,
                       ItemUnitPrice= i.ItemUnitPrice,
                   }).HasKey(x => x.Uid);
    }
}
