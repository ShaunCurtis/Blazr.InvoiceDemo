/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Core;

public readonly struct ApplicationConstants
{
    public const string Uid = "Uid";
    public const string Date = "Date";

    //Customer
    public const string CustomerName = "CustomerName";

    //Product
    public const string ProductName = "ProductName";
    public const string ProductCode = "ProductCode";
    public const string ProductUnitPrice = "ProductUnitPrice";

    //Invoice
    public const string InvoiceNumber = "InvoiceNumber";
    public const string InvoiceDate = "InvoiceDate";
    public const string CustomerUid = "CustomerUid";
    public const string InvoicePrice = "InvoicePrice";

    public const string ByCustomerUid = "ByCustomerUid";
    public const string ByInvoiceMonth = "ByInvoiceMonth";

    //InvoiceItem
    public const string InvoiceUid = "InvoiceUid";
    public const string ProductUid = "ProductUid";
    public const string ItemQuantity = "ItemQuantity";
    public const string ItemUnitPrice = "ItemUnitPrice";

    public const string ByInvoiceUid = "ByInvoiceUid";
    public const string ByProductUid = "ByProductUid";

}
