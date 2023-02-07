/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.Core;

public struct MessageStoreHandle
{
    private readonly Guid _handleUid;
    private readonly BlazrValidationMessageStore _store;

    internal MessageStoreHandle(BlazrValidationMessageStore store)
    {
        _handleUid = Guid.NewGuid();
        _store = store;
    }

    public void AddMessage(ValidationMessage message)
        => _store.Add(message with { HandleUid = _handleUid });

    public void AddMessage(FieldReference field, string message)
        => _store.Add(new ValidationMessage(HandleUid: _handleUid, Field: field, Message: message));

    public void AddMessages(FieldReference field, IEnumerable<string> messages)
    {
        foreach (var message in messages)
            _store.Add(new ValidationMessage(HandleUid: _handleUid, Field: field, Message: message));
    }

    public void ClearMessages(FieldReference field)
        => _store.ClearMessages(_handleUid, field);

    public void ClearMessages()
        => _store.ClearMessages(_handleUid);
}
