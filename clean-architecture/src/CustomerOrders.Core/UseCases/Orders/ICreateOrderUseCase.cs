using CustomerOrders.Core.Domain;

namespace CustomerOrders.Core.UseCases.Orders;

// Returns null when the customer does not exist.
// Throws InactiveCustomerException (-> 409) when the customer is not active.
public interface ICreateOrderUseCase
{
    Order? Create(int customerId, decimal amount);
}
