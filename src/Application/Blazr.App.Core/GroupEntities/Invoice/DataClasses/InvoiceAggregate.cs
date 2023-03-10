/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Core;

public sealed class InvoiceAggregate : IGuidIdentity
{
    private Invoice _baseInvoice { get; set; } = new Invoice();
    private readonly List<InvoiceItem> _baseInvoiceItems = new();
    private readonly List<InvoiceItem> _invoiceItems = new();
    private readonly List<InvoiceItem> _newInvoiceItems = new();
    private readonly List<InvoiceItem> _deletedInvoiceItems = new();

    public Guid Uid { get; init; } = Guid.NewGuid();

    public Invoice Invoice { get; private set; } = new Invoice();

    public bool IsNewInvoice { get; private set; }
    public bool InvoiceIsDirty => this.Invoice.Equals(_baseInvoice);

    public IEnumerable<InvoiceItem> InvoiceItems
    {
        get
        {
            var list = _invoiceItems.ToList();
            list.AddRange(_newInvoiceItems);
            return list;
        }
    }

    public IEnumerable<InvoiceItem> UpdatedItems
    {
        get
        {
            var list = new List<InvoiceItem>();

            foreach (var item in _invoiceItems)
            {
                if (!_baseInvoiceItems.Contains(item))
                    list.Add(item);
            }
            return list;
        }
    }

    public IEnumerable<InvoiceItem> DeletedItems
        => _deletedInvoiceItems.ToList();

    public IEnumerable<InvoiceItem> AddedItems
        => _newInvoiceItems.ToList();

    public InvoiceAggregate()
        => this.IsNewInvoice = true;

    public InvoiceAggregate(Invoice? invoice, IEnumerable<InvoiceItem>? items)
    {
        this.IsNewInvoice = invoice is null;

        this.Invoice = invoice ?? new();
        _baseInvoice = this.Invoice with { };

        if (items is not null)
            _invoiceItems.AddRange(items);
    }

    public bool AddInvoiceItem(InvoiceItem item)
        => this.addInvoiceItem(item);

    public bool RemoveInvoiceItem(InvoiceItem item)
        => this.removeInvoiceItem(item);

    public bool UpdateInvoiceItem(InvoiceItem item)
        => this.updateInvoiceItem(item);

    private bool removeInvoiceItem(InvoiceItem item)
    {
        if (_invoiceItems.Remove(item) || _newInvoiceItems.Remove(item))
        {
            _deletedInvoiceItems.Add(item);
            return true;
        }

        return false;
    }

    private bool addInvoiceItem(InvoiceItem item)
    {
        if (this.InvoiceItemExists(item.Uid))
            return false;

        _newInvoiceItems.Add(item);
        return true;
    }

    private bool updateInvoiceItem(InvoiceItem item)
    {
        if (!this.TryGetInvoiceItem(item.Uid, out InvoiceItem? existingItem))
            return false;

        if (_newInvoiceItems.Remove(existingItem))
        {
            _newInvoiceItems.Add(item);
            return true;
        }

        if (_invoiceItems.Remove(existingItem))
        {
            _invoiceItems.Add(item);
            return true;
        }

        return false;
    }

    private bool InvoiceItemExists(Guid uid)
        => this.InvoiceItems.Any(item => item.Uid.Equals(uid));

    private bool TryGetInvoiceItem(Guid uid, [NotNullWhen(true)] out InvoiceItem? item)
    {
        item = this.InvoiceItems.FirstOrDefault(item => item.Uid.Equals(uid));
        return item != null;
    }

    public void SetInvoiceAsSaved()
    {
        _baseInvoice = this.Invoice with { };
        var list = this.InvoiceItems;
        _invoiceItems.Clear();
        _deletedInvoiceItems.Clear();
        _newInvoiceItems.Clear();
        _baseInvoiceItems.Clear();

        _invoiceItems.AddRange(list);
        _baseInvoiceItems.AddRange(list);
    }

    public void ResetInvoice()
    {
        this.Invoice = _baseInvoice with { };
        _invoiceItems.Clear();
        _deletedInvoiceItems.Clear();
        _newInvoiceItems.Clear();

        _invoiceItems.AddRange(_baseInvoiceItems);
    }

    public void SetToNew()
    {
        this.Invoice = new();
        _baseInvoice= new();
        _invoiceItems.Clear();
        _deletedInvoiceItems.Clear();
        _newInvoiceItems.Clear();
        _baseInvoiceItems.Clear();
    }
}
