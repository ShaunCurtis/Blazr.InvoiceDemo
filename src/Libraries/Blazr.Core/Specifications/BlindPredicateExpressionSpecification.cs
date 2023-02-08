/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.Core;

public class BlindPredicateExpressionSpecification<T> : IPredicateExpressionSpecification<T>
{
    public BlindPredicateExpressionSpecification() { }

    public BlindPredicateExpressionSpecification(FilterDefinition filter) { }

    public IEnumerable<T> AsEnumerable(IEnumerable<T> query)
        => query;

    public IQueryable<T> AsQueryAble(IQueryable<T> query)
        => query;

    public bool IsSatisfiedBy(T entity)
        => true;
}
