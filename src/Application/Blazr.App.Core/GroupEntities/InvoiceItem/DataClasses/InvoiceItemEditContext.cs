using System.Diagnostics;
/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Core;

public sealed class InvoiceItemEditContext : RecordEditContextBase<InvoiceItem>
{

    private int _itemQuantity;
    public int ItemQuantity
    {
        get => _itemQuantity;
        set => UpdateifChangedAndNotify(ref _itemQuantity, value, this.ItemQuantity, ApplicationConstants.ItemQuantity);
    }

    private decimal _itemUnitPrice;
    public decimal ItemUnitPrice
    {
        get => _itemUnitPrice;
        set => UpdateifChangedAndNotify(ref _itemUnitPrice, value, this.ItemUnitPrice, ApplicationConstants.ItemUnitPrice);
    }

    private Guid _invoiceUid = Guid.Empty;
    public Guid InvoiceUid
    {
        get => _invoiceUid;
        set => UpdateifChangedAndNotify(ref _invoiceUid, value, this.InvoiceUid, ApplicationConstants.InvoiceUid);
    }

    private Guid _productUid = Guid.Empty;
    public Guid ProductUid
    {
        get => _productUid;
        set => UpdateifChangedAndNotify(ref _productUid, value, this.ProductUid, ApplicationConstants.ProductUid);
    }

    public override void LoadRecord(InvoiceItem record)
    {
        this.BaseRecord = record ??= this.Record with { };
        this.Uid = record.Uid;
        this.ProductUid = record.ProductUid;
        this.InvoiceUid = record.InvoiceUid;
        this.ItemQuantity = record.ItemQuantity;
        this.ItemUnitPrice = record.ItemUnitPrice;
    }

    public override InvoiceItem AsNewRecord()
        => this.Record with { Uid = Guid.NewGuid() };

    public override InvoiceItem Record =>
        new InvoiceItem
        {
            Uid = this.Uid,
            ProductUid = this.ProductUid,
            InvoiceUid = this.InvoiceUid,
            ItemQuantity = this.ItemQuantity,
            ItemUnitPrice = this.ItemUnitPrice,
        };
}
