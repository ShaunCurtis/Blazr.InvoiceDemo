/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using System.ComponentModel.DataAnnotations;

namespace Blazr.App.Infrastructure;

internal sealed record DboInvoice : IGuidIdentity
{
    [Key] public Guid Uid { get; init; } = Guid.Empty;

    public Guid CustomerUid { get; init; } = Guid.Empty;

    public DateOnly InvoiceDate { get; init; } = DateOnly.FromDateTime(DateTime.MinValue);

    public string InvoiceNumber { get; init; } = "Not Set";

    public decimal InvoicePrice { get; init; }
}
