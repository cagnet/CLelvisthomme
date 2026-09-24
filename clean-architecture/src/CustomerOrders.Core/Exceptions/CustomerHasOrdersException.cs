namespace CustomerOrders.Core.Exceptions;

// Raised when deleting a Customer that still has Orders attached.
// Mapped to HTTP 409 Conflict by the API's exception handling middleware.
public sealed class CustomerHasOrdersException()
    : Exception("A customer with orders cannot be deleted.")
{
    public string Code => "customer_has_orders";
}
