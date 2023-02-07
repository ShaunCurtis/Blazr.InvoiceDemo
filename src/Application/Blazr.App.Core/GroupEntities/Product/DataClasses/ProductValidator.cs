/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Core;

public class ProductValidator : AbstractValidator<ProductEditContext>
{
    public ProductValidator()
    {
        this.RuleFor(p => p.ProductCode)
            .NotEmpty()
            .WithMessage("You must enter a Product Code.");

        this.RuleFor(p => p.ProductName)
            .MinimumLength(3)
            .NotEmpty()
            .WithMessage("Must be at least 3 characters.");

        this.RuleFor(p => p.ProductUnitPrice)
            .GreaterThan(0)
            .WithMessage("Must be greater than £0.00.");
    }
}
