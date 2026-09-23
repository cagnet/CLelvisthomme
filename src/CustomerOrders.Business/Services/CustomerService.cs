using CustomerOrders.Business.Entities;
using CustomerOrders.Business.Exceptions;
using CustomerOrders.Business.Repositories;

namespace CustomerOrders.Business.Services;

public sealed class CustomerService(ICustomerRepository customers, IOrderRepository orders)
{
    public IReadOnlyCollection<Customer> ReadAll() => customers.GetAll();
    public Customer? ReadOne(int id) => customers.GetById(id);
    public IReadOnlyCollection<Order>? ReadOrders(int id) => customers.GetById(id) is null ? null : orders.GetByCustomerId(id);

    public Customer Create(string name, bool isActive)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required.", nameof(name));
        return customers.Add(name.Trim(), isActive);
    }

    public Customer? Update(int id, string? name, bool? isActive)
    {
        var customer = customers.GetById(id);
        if (customer is null) return null;
        if (name is not null && string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot be empty.", nameof(name));
        customer = customer with { Name = name?.Trim() ?? customer.Name, IsActive = isActive ?? customer.IsActive };
        customers.Update(customer);
        return customer;
    }

    public bool Delete(int id)
    {
        if (orders.ExistsForCustomer(id))
            throw new BusinessRuleException("customer_has_orders", "A customer with orders cannot be deleted.");
        return customers.Delete(id);
    }
}
