using CustomerOrders.Core.Exceptions;
using CustomerOrders.Core.Services;
using CustomerOrders.Tests.Fakes;

namespace CustomerOrders.Tests;

public sealed class OrderServiceTests
{
    [Fact]
    public void Create_ThrowsInactiveCustomerException_WhenCustomerIsNotActive()
    {
        var customers = new FakeCustomerRepository();
        var orders = new FakeOrderRepository();
        var service = new OrderService(customers, orders);
        var customer = customers.Add("Doe", null, null, null, isActive: false);

        var exception = Assert.Throws<InactiveCustomerException>(() => service.Create(customer.Id, 10m));
        Assert.Equal("inactive_customer", exception.Code);
    }

    [Fact]
    public void Create_ThrowsArgumentOutOfRangeException_WhenAmountIsNotPositive()
    {
        var customers = new FakeCustomerRepository();
        var orders = new FakeOrderRepository();
        var service = new OrderService(customers, orders);
        var customer = customers.Add("Doe", null, null, null, isActive: true);

        Assert.Throws<ArgumentOutOfRangeException>(() => service.Create(customer.Id, 0m));
        Assert.Throws<ArgumentOutOfRangeException>(() => service.Create(customer.Id, -5m));
    }

    [Fact]
    public void Create_Succeeds_WhenCustomerIsActiveAndAmountIsPositive()
    {
        var customers = new FakeCustomerRepository();
        var orders = new FakeOrderRepository();
        var service = new OrderService(customers, orders);
        var customer = customers.Add("Doe", null, null, null, isActive: true);

        var order = service.Create(customer.Id, 42m);

        Assert.NotNull(order);
        Assert.Equal(42m, order!.Amount);
        Assert.Equal(customer.Id, order.CustomerId);
    }
}
