/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.Core;

public class FieldChangedEventArgs : EventArgs
{
    public bool IsDirty { get; init; }
    public FieldReference Field { get; init; }

    private FieldChangedEventArgs(bool isDirty, FieldReference field)
    {
        this.IsDirty = isDirty;
        this.Field = field;
    }

    public static FieldChangedEventArgs Create(bool isDirty, FieldReference field)
        => new FieldChangedEventArgs(isDirty, field);
}
