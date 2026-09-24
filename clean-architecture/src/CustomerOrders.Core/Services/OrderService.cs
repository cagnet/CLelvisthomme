using CustomerOrders.Core.Domain;
using CustomerOrders.Core.Exceptions;
using CustomerOrders.Core.Ports;
using CustomerOrders.Core.UseCases.Orders;

namespace CustomerOrders.Core.Services;

public sealed class OrderService(ICustomerRepository customers, IOrderRepository orders)
    : IGetOrderUseCase, ICreateOrderUseCase, IUpdateOrderUseCase, IDeleteOrderUseCase
{
    public IReadOnlyCollection<Order> GetAll() => orders.GetAll();

    public Order? GetById(int id) => orders.GetById(id);

    public Order? Create(int customerId, decimal amount)
    {
        var customer = customers.GetById(customerId);
        if (customer is null) return null;
        if (!customer.IsActive) throw new InactiveCustomerException();
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
