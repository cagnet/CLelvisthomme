using CustomerOrders.Core.Domain;

namespace CustomerOrders.Core.UseCases.Customers;

public interface IGetCustomerUseCase
{
    IReadOnlyCollection<Customer> GetAll();
    Customer? GetById(int id);
    IReadOnlyCollection<Order>? GetOrders(int customerId);
}
