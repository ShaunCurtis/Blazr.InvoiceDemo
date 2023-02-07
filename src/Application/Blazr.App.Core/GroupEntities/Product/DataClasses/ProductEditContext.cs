/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Core;

public sealed class ProductEditContext : RecordEditContextBase<Product>
{
    private string _productName = String.Empty;
    public string ProductName
    {
        get => _productName;
        set => UpdateifChangedAndNotify(ref _productName!, value, this.ProductName, ApplicationConstants.ProductName);
    }

    private string _productCode = String.Empty;
    public string ProductCode
    {
        get => _productCode;
        set => UpdateifChangedAndNotify(ref _productCode!, value, this.ProductCode, ApplicationConstants.ProductCode);
    }

    private decimal _productUnitPrice;
    public decimal ProductUnitPrice
    {
        get => _productUnitPrice;
        set => UpdateifChangedAndNotify(ref _productUnitPrice, value, this.ProductUnitPrice, ApplicationConstants.ProductUnitPrice);
    }

    public override void LoadRecord(Product record)
    {
        this.BaseRecord = record ??= this.Record with { };
        this.Uid = record.Uid;
        this.ProductCode= record.ProductCode;
        this.ProductName= record.ProductName;
        this.ProductUnitPrice = record.ProductUnitPrice;
    }

    public override Product AsNewRecord()
        => this.Record with { Uid = Guid.NewGuid() };

    public override Product Record =>
        new Product
        {
            Uid = this.Uid,
            ProductCode = this.ProductCode,
            ProductName = this.ProductName,
            ProductUnitPrice = this.ProductUnitPrice,
        };
}
