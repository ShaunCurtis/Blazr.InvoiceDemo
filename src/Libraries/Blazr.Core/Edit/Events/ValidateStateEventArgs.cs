/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.Core;

public class ValidateStateEventArgs : EventArgs
{
    public FieldReference? Field { get; init; }

    public ValidateStateEventArgs(FieldReference? field)
        => Field = field;

    public static ValidateStateEventArgs Create(FieldReference? field)
        => new ValidateStateEventArgs(field);
}
