/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Core;

public sealed class InvoiceData : IGuidIdentity
{
    private InvoiceView _baseInvoice { get; set; } = new InvoiceView();
    private readonly List<InvoiceItemView> _baseInvoiceItems = new();
    private readonly List<InvoiceItemView> _invoiceItems = new();
    private readonly List<InvoiceItemView> _newInvoiceItems = new();
    private readonly List<InvoiceItemView> _deletedInvoiceItems = new();

    public Guid Uid { get; init; } = Guid.NewGuid();

    public InvoiceView Invoice { get; private set; } = new InvoiceView();

    public bool IsNewInvoice { get; private set; }
    public bool InvoiceIsDirty => this.Invoice.Equals(_baseInvoice);

    public IEnumerable<InvoiceItemView> InvoiceItems
    {
        get
        {
            var list = _invoiceItems.ToList();
            list.AddRange(_newInvoiceItems);
            return list;
        }
    }

    public IEnumerable<InvoiceItemView> UpdatedItems
    {
        get
        {
            var list = new List<InvoiceItemView>();

            foreach (var item in _invoiceItems)
            {
                if (!_baseInvoiceItems.Contains(item))
                    list.Add(item);
            }
            return list;
        }
    }

    public IEnumerable<InvoiceItemView> DeletedItems
        => _deletedInvoiceItems.ToList();

    public IEnumerable<InvoiceItemView> AddedItems
        => _newInvoiceItems.ToList();

    public InvoiceData()
        => this.IsNewInvoice = true;

    public InvoiceData(InvoiceView? invoice, IEnumerable<InvoiceItemView>? items)
    {
        this.IsNewInvoice = invoice is null;

        this.Invoice = invoice ?? new();
        _baseInvoice = this.Invoice with { };

        if (items is not null)
            _invoiceItems.AddRange(items);
    }

    public bool AddInvoiceItem(InvoiceItemView item)
        => this.addInvoiceItem(item);

    public bool RemoveInvoiceItem(InvoiceItemView item)
        => this.removeInvoiceItem(item);

    public bool UpdateInvoiceItem(InvoiceItemView item)
        => this.updateInvoiceItem(item);

    private bool removeInvoiceItem(InvoiceItemView item)
    {
        if (_invoiceItems.Remove(item) || _newInvoiceItems.Remove(item))
        {
            _deletedInvoiceItems.Add(item);
            return true;
        }

        return false;
    }

    private bool addInvoiceItem(InvoiceItemView item)
    {
        if (this.InvoiceItemExists(item.Uid))
            return false;

        _newInvoiceItems.Add(item);
        return true;
    }

    private bool updateInvoiceItem(InvoiceItemView item)
    {
        if (!this.TryGetInvoiceItem(item.Uid, out InvoiceItemView? existingItem))
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

    private bool TryGetInvoiceItem(Guid uid, [NotNullWhen(true)] out InvoiceItemView? item)
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
