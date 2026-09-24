using CustomerOrders.Core.Domain;

namespace CustomerOrders.Core.Ports;

public interface IOrderRepository
{
    IReadOnlyCollection<Order> GetAll();
    IReadOnlyCollection<Order> GetByCustomerId(int customerId);
    Order? GetById(int id);
    Order Add(int customerId, decimal amount, DateTime createdAt);
    void Update(Order order);
    bool Delete(int id);
    bool ExistsForCustomer(int customerId);
}
