using CustomerOrders.Core.Domain;
using CustomerOrders.Core.Ports;

namespace CustomerOrders.Tests.Fakes;

public sealed class FakeOrderRepository : IOrderRepository
{
    private readonly Dictionary<int, Order> _orders = new();
    private int _nextId;

    public IReadOnlyCollection<Order> GetAll() => _orders.Values.ToArray();

    public IReadOnlyCollection<Order> GetByCustomerId(int customerId) =>
        _orders.Values.Where(o => o.CustomerId == customerId).ToArray();

    public Order? GetById(int id) => _orders.GetValueOrDefault(id);

    public Order Add(int customerId, decimal amount, DateTime createdAt)
    {
        var order = new Order(++_nextId, customerId, amount, createdAt);
        _orders[order.Id] = order;
        return order;
    }

    public void Update(Order order) => _orders[order.Id] = order;

    public bool Delete(int id) => _orders.Remove(id);

    public bool ExistsForCustomer(int customerId) => _orders.Values.Any(o => o.CustomerId == customerId);
}
