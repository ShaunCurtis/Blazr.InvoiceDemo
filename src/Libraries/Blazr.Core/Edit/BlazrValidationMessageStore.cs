/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.Core;

public record ValidationMessage(Guid HandleUid, FieldReference Field, string Message);

public class BlazrValidationMessageStore : IEnumerable<ValidationMessage>
{
    private readonly List<ValidationMessage> _messages = new List<ValidationMessage>();

    internal void Add(ValidationMessage message)
        => _messages.Add(message);

    internal void ClearMessages(Guid handleUid)
    {
        var messagesToDelete = _messages.Where(item => item.HandleUid.Equals(handleUid)).ToList();
        if (messagesToDelete is not null)
            foreach (var message in messagesToDelete)
                _messages.Remove(message);
    }

    internal void ClearMessages(Guid handleUid, FieldReference field)
    {
        var messagesToDelete = _messages.Where(item => item.HandleUid.Equals(handleUid) && item.Field.Equals(field)).ToList();
        if (messagesToDelete is not null)
            foreach (var message in messagesToDelete)
                _messages.Remove(message);
    }

    internal void Reset()
        => _messages.Clear();

    public MessageStoreHandle GetHandle()
        => new MessageStoreHandle(this);

    public IEnumerable<string> GetMessages(FieldReference? field = null)
    {
        var query = _messages.AsEnumerable();

        if (field is not null)
            query = query.Where(item => item.Field.Equals(field));

        return query.Select(item => item.Message);
    }

    public bool HasMessages(FieldReference? field = null)
        => field is null
            ? _messages.Any()
            : _messages.Any(item => item.Field.Equals(field));

    public IEnumerator<ValidationMessage> GetEnumerator()
        => _messages.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}