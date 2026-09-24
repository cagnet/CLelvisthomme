using CustomerOrders.Core.Domain;
using CustomerOrders.Core.Exceptions;
using CustomerOrders.Core.Ports;
using CustomerOrders.Core.UseCases.Customers;

namespace CustomerOrders.Core.Services;

// One service implementing several fine-grained IUseCase interfaces — the
// scope here (two resources, no divergent read/write concerns) does not
// justify a full command/query split.
public sealed class CustomerService(ICustomerRepository customers, IOrderRepository orders)
    : IGetCustomerUseCase, ICreateCustomerUseCase, IUpdateCustomerUseCase, IDeleteCustomerUseCase
{
    public IReadOnlyCollection<Customer> GetAll() => customers.GetAll();

    public Customer? GetById(int id) => customers.GetById(id);

    public IReadOnlyCollection<Order>? GetOrders(int customerId) =>
        customers.GetById(customerId) is null ? null : orders.GetByCustomerId(customerId);

    public Customer Create(string name, string? firstName, string? email, string? address, bool isActive)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.", nameof(name));
        return customers.Add(name.Trim(), firstName, email, address, isActive);
    }

    public Customer? Update(int id, string? name, string? firstName, string? email, string? address, bool? isActive)
    {
        var customer = customers.GetById(id);
        if (customer is null) return null;
        if (name is not null && string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty.", nameof(name));

        customer = customer with
        {
            Name = name?.Trim() ?? customer.Name,
            FirstName = firstName ?? customer.FirstName,
            Email = email ?? customer.Email,
            Address = address ?? customer.Address,
            IsActive = isActive ?? customer.IsActive
        };
        customers.Update(customer);
        return customer;
    }

    public bool Delete(int id)
    {
        if (orders.ExistsForCustomer(id))
            throw new CustomerHasOrdersException();
        return customers.Delete(id);
    }
}
