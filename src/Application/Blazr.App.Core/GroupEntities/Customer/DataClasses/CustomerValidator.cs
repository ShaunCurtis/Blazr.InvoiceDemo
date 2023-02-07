/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Core;

public class CustomerValidator : AbstractValidator<CustomerEditContext>
{
    public CustomerValidator()
    {
        this.RuleFor(p => p.CustomerName)
            .NotEmpty()
            .WithMessage("You must enter a Customer Name.");

        this.RuleFor(p => p.CustomerName)
            .MinimumLength(3)
            .NotEmpty()
            .WithMessage("Must be at least 3 characters.");
    }
}
