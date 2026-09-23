using CustomerOrders.Business.Entities;
using CustomerOrders.Business.Exceptions;
using CustomerOrders.Business.Repositories;

namespace CustomerOrders.Business.Services;

public sealed class OrderService(ICustomerRepository customers, IOrderRepository orders)
{
    public IReadOnlyCollection<Order> ReadAll() => orders.GetAll();
    public Order? ReadOne(int id) => orders.GetById(id);

    public Order? Create(int customerId, decimal amount)
    {
        var customer = customers.GetById(customerId);
        if (customer is null) return null;
        if (!customer.IsActive)
            throw new BusinessRuleException("inactive_customer", "An order cannot be created for an inactive customer.");
        if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be greater than zero.");
        return orders.Add(customerId, amount, DateTime.UtcNow);
    }

    public Order? Update(int id, decimal? amount)
    {
        var order = orders.GetById(id);
        if (order is null) return null;
        if (amount is <= 0) throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be greater than zero.");
        order = order with { Amount = amount ?? order.Amount };
        orders.Update(order);
        return order;
    }

    public bool Delete(int id) => orders.Delete(id);
}
