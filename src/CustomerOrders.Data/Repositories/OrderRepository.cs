using CustomerOrders.Business.Entities;
using CustomerOrders.Business.Repositories;
using CustomerOrders.Data.InMemory;

namespace CustomerOrders.Data.Repositories;

public sealed class OrderRepository(InMemoryDatabase database) : IOrderRepository
{
    public IReadOnlyCollection<Order> GetAll() => database.Orders.Values.OrderBy(x => x.Id).ToArray();
    public IReadOnlyCollection<Order> GetByCustomerId(int customerId) => database.Orders.Values.Where(x => x.CustomerId == customerId).OrderBy(x => x.Id).ToArray();
    public Order? GetById(int id) => database.Orders.GetValueOrDefault(id);
    public Order Add(int customerId, decimal amount, DateTime createdAt)
    {
        var order = new Order(database.NextOrderId(), customerId, amount, createdAt);
        database.Orders[order.Id] = order;
        return order;
    }
    public void Update(Order order) => database.Orders[order.Id] = order;
    public bool Delete(int id) => database.Orders.TryRemove(id, out _);
    public bool ExistsForCustomer(int customerId) => database.Orders.Values.Any(x => x.CustomerId == customerId);
}
