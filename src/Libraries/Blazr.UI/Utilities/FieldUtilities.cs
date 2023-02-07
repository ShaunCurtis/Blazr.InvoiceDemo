/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.UI;

public static class FieldUtilities
{

    /// <summary>
    /// Method that gets a FieldReference from an expression in the format `() => object.property`
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="accessor"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static FieldReference ParseAccessor<T>(Expression<Func<T>> accessor)
    {
        object model;
        var accessorBody = accessor.Body;

        // Unwrap casts to object
        if (accessorBody is UnaryExpression unaryExpression
            && unaryExpression.NodeType == ExpressionType.Convert
            && unaryExpression.Type == typeof(object))
        {
            accessorBody = unaryExpression.Operand;
        }

        if (!(accessorBody is MemberExpression memberExpression))
            throw new ArgumentException($"The provided expression contains a {accessorBody.GetType().Name} which is not supported. {nameof(FieldIdentifier)} only supports simple member accessors (fields, properties) of an object.");

        // Identify the field name. We don't mind whether it's a property or field, or even something else.
        string fieldName = memberExpression.Member.Name;

        // Get a reference to the model object
        // i.e., given a value like "(something).MemberName", determine the runtime value of "(something)",
        if (memberExpression.Expression is ConstantExpression constantExpression)
        {
            if (constantExpression.Value is null)
                throw new ArgumentException("The provided expression must evaluate to a non-null value.");

            model = constantExpression.Value;
        }
        else if (memberExpression.Expression != null)
        {
            var modelLambda = Expression.Lambda(memberExpression.Expression);
            var modelLambdaCompiled = (Func<object?>)modelLambda.Compile();
            var result = modelLambdaCompiled();
            if (result is null)
                throw new ArgumentException("The provided expression must evaluate to a non-null value.");

            model = result;
        }
        else
            throw new ArgumentException($"The provided expression contains a {accessorBody.GetType().Name} which is not supported. {nameof(FieldIdentifier)} only supports simple member accessors (fields, properties) of an object.");

        return new FieldReference(model, fieldName);
    }

    /// <summary>
    /// Method to convert the supplied object into a MarkupString
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static MarkupString GetAsMarkup(object? value)
    {
        switch (value)
        {
            case MarkupString mValue:
                return mValue;

            case string sValue:
                return (MarkupString)(sValue);

            case null:
                return new MarkupString(string.Empty);

            default:
                return new MarkupString(value?.ToString() ?? String.Empty);
        }
    }
}
