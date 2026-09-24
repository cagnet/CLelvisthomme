using CustomerOrders.Core.Exceptions;
using CustomerOrders.Core.Services;
using CustomerOrders.Tests.Fakes;

namespace CustomerOrders.Tests;

public sealed class CustomerServiceTests
{
    [Fact]
    public void Delete_ThrowsCustomerHasOrdersException_WhenCustomerStillHasOrders()
    {
        var customers = new FakeCustomerRepository();
        var orders = new FakeOrderRepository();
        var service = new CustomerService(customers, orders);

        var customer = service.Create("Doe", null, null, null, true);
        orders.Add(customer.Id, 10m, DateTime.UtcNow);

        var exception = Assert.Throws<CustomerHasOrdersException>(() => service.Delete(customer.Id));
        Assert.Equal("customer_has_orders", exception.Code);
    }

    [Fact]
    public void Delete_Succeeds_WhenCustomerHasNoOrders()
    {
        var customers = new FakeCustomerRepository();
        var orders = new FakeOrderRepository();
        var service = new CustomerService(customers, orders);

        var customer = service.Create("Doe", null, null, null, true);

        var deleted = service.Delete(customer.Id);

        Assert.True(deleted);
        Assert.Null(service.GetById(customer.Id));
    }

    [Fact]
    public void Create_ThrowsArgumentException_WhenNameIsBlank()
    {
        var service = new CustomerService(new FakeCustomerRepository(), new FakeOrderRepository());

        Assert.Throws<ArgumentException>(() => service.Create("   ", null, null, null, true));
    }
}
