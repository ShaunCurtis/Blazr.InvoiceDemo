/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.Core;

public interface IEditContext
{
    public event EventHandler<FieldChangedEventArgs>? FieldChanged;
    public event EventHandler<ValidateStateEventArgs>? ValidationStateUpdated;
    public event EventHandler<ValidationRequestEventArgs>? ValidationRequested;
    public event EventHandler? EditStateChanged;

    public bool IsDirty { get; }
    public bool IsNew { get; }
    public bool IsValid { get; }
    public bool HasValidationMessages(FieldReference field);
    public bool HasValidationMessages();
    public bool FieldHasChanged(FieldReference field);
    public IEnumerable<string> GetValidationMessages(FieldReference field);
    public IEnumerable<string> GetValidationMessages();
    public MessageStoreHandle GetMessageStoreHandle();
    public void NotifyValidationStateUpdated(object? sender, FieldReference? field);
    public object GetModel();
}
