/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
using System.Linq.Expressions;

namespace Blazr.App.Core;

public class CustomerSorter : RecordSortBase<Customer>, IRecordSorter<Customer>
{
    public  IQueryable<Customer> AddSortToQuery(string fieldName, IQueryable<Customer> query, bool sortDescending)
        => fieldName switch
        {
            _ => Sort(query, sortDescending, OnCustomerName)
        };

    private static Expression<Func<Customer, object>> OnCustomerName => item => item.CustomerName ?? string.Empty;
}
