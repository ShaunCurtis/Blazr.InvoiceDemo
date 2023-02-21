/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.App.Core;

public static class CustomerMapperExtensions
{
    public static DboCustomer MapToDbo(this Customer customer)
        => new()
        {
            CustomerName = customer.CustomerName,
            Uid = customer.Uid,
        };
}
