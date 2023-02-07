/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Core;

public class InvoiceItemValidator : AbstractValidator<InvoiceItemEditContext>
{
    public InvoiceItemValidator()
    {
        this.RuleFor(p => p.ProductUid)
            .NotEmpty()
            .WithMessage("You must select a product.");

        this.RuleFor(p => p.InvoiceUid)
            .NotEmpty()
            .WithMessage("You must select an Invoice.");

        this.RuleFor(p => p.ItemQuantity)
            .GreaterThan(-1)
            .WithMessage("You must select 0 or more items");
    }
}
