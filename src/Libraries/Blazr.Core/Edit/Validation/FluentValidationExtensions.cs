/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
using FluentValidation.Internal;

namespace Blazr.Core;

public static class FluentValidationExtensions
{
    public static void AddFluentValidation<TValidator>(this IEditContext editContext) where TValidator : IValidator
    {
        if (editContext is null)
            throw new ArgumentNullException(nameof(editContext));

        var handle = editContext.GetMessageStoreHandle();
        var validator = (Activator.CreateInstance(typeof(TValidator)) as IValidator);

        if (validator is not null)
        {
            editContext.ValidationRequested += (sender, eventargs) => ValidateModel(editContext, validator!, handle);
            editContext.FieldChanged += (sender, eventArgs) => ValidateField(editContext, validator!, handle, eventArgs.Field);
        }
    }

    private static void ValidateModel(IEditContext editContext, IValidator validator, MessageStoreHandle handle)
    {
        var context = new ValidationContext<object>(editContext);

        var results = validator.Validate(context);
        handle.ClearMessages();

        if (results.IsValid)
            return;

        foreach (var result in results.Errors)
            handle.AddMessage(new FieldReference(editContext, result.PropertyName), result.ErrorMessage);

        editContext.NotifyValidationStateUpdated(null, null);
    }

    private static void ValidateField(IEditContext editContext, IValidator validator, MessageStoreHandle handle, FieldReference field)
    {
        var properties = new[] { field.FieldName };
        var context = new ValidationContext<object>(editContext, new PropertyChain(), new MemberNameValidatorSelector(properties));

        var results = validator.Validate(context);
        handle.ClearMessages(field);

        foreach (var result in results.Errors)
            handle.AddMessage(field, result.ErrorMessage);

        editContext.NotifyValidationStateUpdated(null, field);
    }
}
