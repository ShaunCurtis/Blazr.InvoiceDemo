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

    private string _invoiceNumber = String.Empty;
    public string InvoiceNumber
    {
        get => _invoiceNumber;
        set => UpdateifChangedAndNotify(ref _invoiceNumber!, value, this.InvoiceNumber, ApplicationConstants.InvoiceNumber);
    }

    private Guid _productUid = Guid.Empty;
    public Guid ProductUid
    {
        get => _productUid;
        set => UpdateifChangedAndNotify(ref _productUid, value, this.ProductUid, ApplicationConstants.ProductUid);
    }

    private string _productCode = String.Empty;
    public string ProductCode
    {
        get => _productCode;
        set => UpdateifChangedAndNotify(ref _productCode!, value, this.ProductCode, ApplicationConstants.ProductCode);
    }

    private string _productName = String.Empty;
    public string ProductName
    {
        get => _productName;
        set => UpdateifChangedAndNotify(ref _productName!, value, this.ProductName, ApplicationConstants.ProductName);
    }

    public override void LoadRecord(InvoiceItem record)
    {
        this.BaseRecord = record ??= this.Record with { };
        this.Uid = record.Uid;
        this.ProductUid = record.ProductUid;
        this.ProductCode = record.ProductCode;
        this.ProductName = record.ProductName;
        this.InvoiceUid = record.InvoiceUid;
        this.InvoiceNumber = record.InvoiceNumber;
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
            ProductName = this.ProductName,
            ProductCode = this.ProductCode,
            InvoiceUid = this.InvoiceUid,
            InvoiceNumber = this.InvoiceNumber,
            ItemQuantity = this.ItemQuantity,
            ItemUnitPrice = this.ItemUnitPrice,
        };
}
