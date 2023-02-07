/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.Core;

/// <summary>
/// Base abstact class for Record edit context implementations that implement
/// most of the boilerplate code
/// </summary>
/// <typeparam name="TRecord"></typeparam>

public abstract class RecordEditContextBase<TRecord> : IRecordEditContext<TRecord>, IEditContext
    where TRecord : class, new()
{
    private bool _currentDirtyState;
    public TRecord BaseRecord { get; protected set; } = new();
    public virtual Guid Uid { get; protected set; }
    public virtual TRecord Record => new();
    public virtual bool IsDirty => !BaseRecord.Equals(Record);
    public bool IsNew => Uid == Guid.Empty;
    public bool IsValid => !ValidationMessageStore.HasMessages();

    public event EventHandler<FieldChangedEventArgs>? FieldChanged;
    public event EventHandler? EditStateChanged;
    public event EventHandler<ValidateStateEventArgs>? ValidationStateUpdated;
    public event EventHandler<ValidationRequestEventArgs>? ValidationRequested;

    public readonly BlazrPropertyStateStore PropertyStateStore = new();
    public readonly BlazrValidationMessageStore ValidationMessageStore = new();

    public RecordEditContextBase()
        => LoadRecord(new());

    public void Load(TRecord record, bool notify = true)
    {
        this.BaseRecord = record;
        this.LoadRecord(record);
        _currentDirtyState = false;
        if (notify)
            this.EditStateChanged?.Invoke(this, EventArgs.Empty);
    }

    public abstract void LoadRecord(TRecord record);

    public abstract TRecord AsNewRecord();

    /// <summary>
    /// Method to reset the eit state to the original state
    /// </summary>
    public void Reset()
    {
        Load(BaseRecord);
        _currentDirtyState = false;
        this.EditStateChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Method to reset the edit state to the new values
    /// Resets the base record and the internal state to clean
    /// </summary>
    public void SetAsSaved()
    {
        Load(Record);
        _currentDirtyState = false;
        this.EditStateChanged?.Invoke(this, EventArgs.Empty);
    }

    protected bool UpdateifChangedAndNotify<TType>(ref TType currentValue, TType value, TType originalValue, string fieldName)
    {
        // constructs a FieldReference object for the property we are updating
        var field = new FieldReference(this, fieldName);

        // Checks if the value has changed
        var hasChanged = !value?.Equals(currentValue) ?? currentValue is not null;

        // Checks if the value has changed from the original
        var hasChangedFromOriginal = !value?.Equals(originalValue) ?? originalValue is not null;

        if (hasChanged)
        {
            // Set the current value to the new value
            currentValue = value;

            // Updates the State store
            if (hasChangedFromOriginal)
                this.PropertyStateStore.Add(field);
            else
                this.PropertyStateStore.ClearState(field);

            // Notify to raise the FieldChange event
            NotifyFieldChanged(field, hasChangedFromOriginal);
        }

        return hasChanged;
    }

    public bool HasValidationMessages(FieldReference field)
        => ValidationMessageStore.HasMessages(field);

    public bool HasValidationMessages()
        => ValidationMessageStore.HasMessages();

    public IEnumerable<string> GetValidationMessages(FieldReference field)
        => ValidationMessageStore.GetMessages(field);

    public IEnumerable<string> GetValidationMessages()
        => ValidationMessageStore.GetMessages();

    public MessageStoreHandle GetMessageStoreHandle()
        => this.ValidationMessageStore.GetHandle();

    public void NotifyValidationStateUpdated(object? sender, FieldReference? field)
        => this.ValidationStateUpdated?.Invoke(this, new ValidateStateEventArgs(field));

    public void NotifyValidationRequested(object? sender, FieldReference? field)
        => this.ValidationRequested?.Invoke(this, ValidationRequestEventArgs.Create(field));

    public bool FieldHasChanged(FieldReference field)
        => this.PropertyStateStore.Any(item => item == field);

    public object GetModel()
        => this;

    protected void NotifyFieldChanged(FieldReference field, bool isFieldDirty)
    {
        // Raise thw Field changed event
        // and if we have a state change raise the EditStateChanged as well
        this.FieldChanged?.Invoke(this, FieldChangedEventArgs.Create(this.IsDirty, field));
        if (this.IsDirty != _currentDirtyState)
        {
            _currentDirtyState = this.IsDirty;
            this.EditStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
