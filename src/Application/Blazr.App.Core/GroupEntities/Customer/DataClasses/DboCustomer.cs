/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using System.ComponentModel.DataAnnotations;

namespace Blazr.App.Core;

public sealed record DboCustomer : IGuidIdentity
{
    [Key] public Guid Uid { get; init; } = Guid.Empty;

    public string CustomerName { get; init; } = "Not Set";
}
