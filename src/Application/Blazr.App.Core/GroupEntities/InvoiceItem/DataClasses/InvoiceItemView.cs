/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using System.ComponentModel.DataAnnotations;

namespace Blazr.App.Core;

public sealed record InvoiceItemView : IGuidIdentity
{
    [Key] public Guid Uid { get; init; } = Guid.Empty;

    public Guid InvoiceUid { get; init; } = Guid.Empty;

    public Guid ProductUid { get; init; } = Guid.Empty;

    public string InvoiceNumber { get; init; } = "Not Set";

    public string ProductCode { get; init; } = "Not Set";

    public string ProductName { get; init; } = "Not Set";

    public int ItemQuantity { get; init; }

    public decimal ItemUnitPrice { get; init; }
}

