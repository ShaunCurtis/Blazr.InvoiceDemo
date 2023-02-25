/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Core;

public sealed class InvoiceEditContext : RecordEditContextBase<Invoice>
{
    private decimal _invoicePrice;
    public decimal InvoicePrice
    {
        get => _invoicePrice;
        set => UpdateifChangedAndNotify(ref _invoicePrice!, value, this.InvoicePrice, ApplicationConstants.InvoicePrice);
    }

    private string _invoiceNumber = String.Empty;
    public string InvoiceNumber
    {
        get => _invoiceNumber;
        set => UpdateifChangedAndNotify(ref _invoiceNumber!, value, this.InvoiceNumber, ApplicationConstants.InvoiceNumber);
    }

    private DateOnly? _invoiceDate;
    public DateOnly? InvoiceDate
    {
        get => _invoiceDate;
        set => UpdateifChangedAndNotify(ref _invoiceDate!, value, this.InvoiceDate, ApplicationConstants.InvoiceDate);
    }

    private Guid _customerUid = Guid.Empty;
    public Guid CustomerUid
    {
        get => _customerUid;
        set => UpdateifChangedAndNotify(ref _customerUid!, value, this.CustomerUid, ApplicationConstants.CustomerUid);
    }

    private string _customerName = String.Empty;
    public string CustomerName
    {
        get => _customerName;
        set => UpdateifChangedAndNotify(ref _customerName!, value, this.CustomerName, ApplicationConstants.CustomerName);
    }

    public override void LoadRecord(Invoice record)
    {
        this.BaseRecord = record ??= this.Record with { };
        this.Uid = record.Uid;
        this.CustomerUid = record.CustomerUid;
        this.CustomerName = record.CustomerName;
        this.InvoiceNumber= record.InvoiceNumber;
        this.InvoiceDate = record.InvoiceDate;
        this.InvoicePrice = record.InvoicePrice;

    }

    public override Invoice AsNewRecord()
        => this.Record with { Uid = Guid.NewGuid() };

    public override Invoice Record =>
        new Invoice
        {
            Uid = this.Uid,
            CustomerUid = this.CustomerUid,
            CustomerName = this.CustomerName,
            InvoiceNumber = this.InvoiceNumber,
            InvoiceDate = this.InvoiceDate ?? DateOnly.MinValue,
            InvoicePrice= this.InvoicePrice,
        };
}
