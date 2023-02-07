/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.Core;

public class ValidationRequestEventArgs : EventArgs
{
    public FieldReference? Field { get; init; }

    private ValidationRequestEventArgs()
    => this.Field = null;

    private ValidationRequestEventArgs(FieldReference? field)
        => this.Field = field;

    public static ValidationRequestEventArgs Create(FieldReference? field)
        => new ValidationRequestEventArgs(field);
}
