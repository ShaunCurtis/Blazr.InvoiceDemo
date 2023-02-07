/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
using System.Linq.Expressions;

namespace Blazr.App.Core;

public class ProductSorter : RecordSortBase<Product>, IRecordSorter<Product>
{
    public  IQueryable<Product> AddSortToQuery(string fieldName, IQueryable<Product> query, bool sortDescending)
        => fieldName switch
        {
            ApplicationConstants.ProductCode => Sort(query, sortDescending, OnProductCode),
            ApplicationConstants.ProductUnitPrice => Sort(query, sortDescending, OnUnitPrice),
            _ => Sort(query, sortDescending, OnProductName)
        };

    private static Expression<Func<Product, object>> OnProductCode => item => item.ProductCode ?? string.Empty;
    private static Expression<Func<Product, object>> OnUnitPrice => item => item.ProductUnitPrice;
    private static Expression<Func<Product, object>> OnProductName => item => item.ProductName ?? string.Empty;
}
