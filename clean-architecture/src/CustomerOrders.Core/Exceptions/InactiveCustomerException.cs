namespace CustomerOrders.Core.Exceptions;

// Raised when an Order is created for a Customer whose IsActive is false.
// Mapped to HTTP 409 Conflict by the API's exception handling middleware.
public sealed class InactiveCustomerException()
    : Exception("An order cannot be created for an inactive customer.")
{
    public string Code => "inactive_customer";
}
