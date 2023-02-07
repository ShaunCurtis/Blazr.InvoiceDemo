/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Core;

public sealed class CustomerEditContext : RecordEditContextBase<Customer>
{
    private string _customerName = String.Empty;
    public string CustomerName
    {
        get => _customerName;
        set => UpdateifChangedAndNotify(ref _customerName, value, this.CustomerName, ApplicationConstants.CustomerName);
    }

    public override void LoadRecord(Customer record)
    {
        this.BaseRecord = record ??= this.Record with { };
        this.Uid = record.Uid;
        this.CustomerName = record.CustomerName;
    }

    public override Customer AsNewRecord()
        => this.Record with { Uid = Guid.NewGuid() };

    public override Customer Record =>
        new Customer
        {
            Uid = this.Uid,
            CustomerName= this.CustomerName,
        };
}
