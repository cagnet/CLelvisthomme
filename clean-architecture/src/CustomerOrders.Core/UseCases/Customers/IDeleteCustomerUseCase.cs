namespace CustomerOrders.Core.UseCases.Customers;

// Throws CustomerHasOrdersException (-> 409) when the customer still has orders.
public interface IDeleteCustomerUseCase
{
    bool Delete(int id);
}
