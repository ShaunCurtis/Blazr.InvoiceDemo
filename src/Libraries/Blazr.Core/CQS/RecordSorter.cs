/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
using System.Reflection;

namespace Blazr.Core;

public class RecordSorter<TRecord>
    where TRecord : class
{
    protected virtual Expression<Func<TRecord, object>>? DefaultSorter => null;

    public IQueryable<TRecord> AddSortToQuery(string fieldName, IQueryable<TRecord> query, bool sortDescending)
        => Sort(query, sortDescending, fieldName);

    protected IQueryable<TRecord> Sort(IQueryable<TRecord> query, bool sortDescending, string field)
    {
        Expression<Func<TRecord, object>>? expression = null;

        if (!TryBuildSortExpression(field, out expression))
            expression = this.DefaultSorter;

        if (expression is not null)
            return sortDescending
            ? query.OrderByDescending(expression)
            : query.OrderBy(expression);

        return query;
    }

    protected static bool TryBuildSortExpression(string sortField, [NotNullWhen(true)] out Expression<Func<TRecord, object>>? expression)
    {
        expression = null;

        Type recordType = typeof(TRecord);
        PropertyInfo sortProperty = recordType.GetProperty(sortField)!;
        if (sortProperty is null)
            return false;

        ParameterExpression parameterExpression = Expression.Parameter(recordType, "item");
        MemberExpression memberExpression = Expression.Property((Expression)parameterExpression, sortField);
        Expression propertyExpression = Expression.Convert(memberExpression, typeof(object));

        expression = Expression.Lambda<Func<TRecord, object>>(propertyExpression, parameterExpression);

        return true;
    }
}

